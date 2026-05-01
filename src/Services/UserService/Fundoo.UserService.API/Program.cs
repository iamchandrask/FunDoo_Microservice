using System.Text;
using FluentValidation;
using MediatR;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

using Fundoo.UserService.Application.Contracts;
using Fundoo.UserService.Application.Features.Users.Commands.RegisterUser;
using Fundoo.UserService.Application.Validators;
using Fundoo.UserService.Infrastructure.Persistence;
using Fundoo.UserService.Infrastructure.Repositories;
using Fundoo.UserService.Infrastructure.Security;

using Shared.Infrastructure.Behaviors;
using Shared.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);


// ========================= LOGGING (SERILOG) =========================

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();


// ========================= CONTROLLERS & SWAGGER =========================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ========================= DATABASE =========================

var connectionString = builder.Configuration.GetConnectionString("UserDb")
    ?? throw new Exception("UserDb connection string is missing");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));


// ========================= MEDIATR =========================

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));


// ========================= FLUENT VALIDATION =========================

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();

// 🔥 Pipeline Behavior (IMPORTANT)
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// ========================= DEPENDENCY INJECTION =========================

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


// ========================= JWT AUTHENTICATION =========================

var jwtSection = builder.Configuration.GetSection("JwtSettings");

var secret = jwtSection["Secret"]
    ?? throw new Exception("JWT Secret is missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddAuthorization();


// ========================= MASSTRANSIT (RABBITMQ) =========================

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"]
            ?? throw new Exception("RabbitMQ Host missing");

        cfg.Host(host, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });
    });
});


// ========================= HEALTH CHECKS =========================

var redisConn = builder.Configuration["Redis:ConnectionString"];

var healthChecks = builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString,
        name: "sqlserver",
        tags: new[] { "db", "sql", "ready" });

if (!string.IsNullOrEmpty(redisConn))
{
    healthChecks.AddRedis(
        redisConn,
        name: "redis",
        tags: new[] { "cache", "ready" });
}


// ========================= BUILD =========================

var app = builder.Build();


// ========================= MIDDLEWARE =========================

// Global Exception Handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Correlation ID (for tracing)
app.UseMiddleware<CorrelationIdMiddleware>();

// Logging
app.UseSerilogRequestLogging();

// Swagger (Dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS (optional but recommended)
app.UseHttpsRedirection();

// Auth
app.UseAuthentication();
app.UseAuthorization();


// ========================= HEALTH ENDPOINTS =========================

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});


// ========================= ENDPOINTS =========================

app.MapControllers();


// ========================= RUN =========================

app.Run();