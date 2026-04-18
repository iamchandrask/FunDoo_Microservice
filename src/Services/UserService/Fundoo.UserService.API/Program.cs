using System.Text;
using Fundoo.UserService.API.Middleware;
using Fundoo.UserService.Application.Contracts;
using Fundoo.UserService.Application.Features.Users.Commands.RegisterUser;
using Fundoo.UserService.Infrastructure.Persistence;
using Fundoo.UserService.Infrastructure.Repositories;
using Fundoo.UserService.Infrastructure.Security;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

// Create builder for configuring services and app pipeline
var builder = WebApplication.CreateBuilder(args);

// ========================= LOGGING (SERILOG) =========================

// Configure Serilog for structured logging
// - Logs will be written to console (can be extended to files, Seq, etc.)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Replace default logging with Serilog
builder.Host.UseSerilog();


// ========================= CONTROLLERS & SWAGGER =========================

// Add support for Controllers (API endpoints)
builder.Services.AddControllers();

// Enables API endpoint discovery (used by Swagger)
builder.Services.AddEndpointsApiExplorer();

// Adds Swagger (API documentation + testing UI)
builder.Services.AddSwaggerGen();


// ========================= DATABASE (EF CORE) =========================

// Register DbContext with SQL Server provider
// - Connection string is fetched from appsettings.json
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDb")));


// ========================= MEDIATR (CQRS) =========================

// Register MediatR and scan assembly for handlers
// - Automatically finds CommandHandlers & QueryHandlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));


// ========================= DEPENDENCY INJECTION =========================

// Register Repository
// - IUserRepository → UserRepository implementation
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register JWT Token Generator
// - Handles token creation logic
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


// ========================= JWT AUTHENTICATION =========================

// Read JWT settings from configuration (appsettings.json)
var jwtSection = builder.Configuration.GetSection("JwtSettings");

// Configure Authentication using JWT Bearer scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Token validation rules
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Validate issuer (who created the token)
            ValidateIssuer = true,

            // Validate audience (who the token is for)
            ValidateAudience = true,

            // Validate token expiration
            ValidateLifetime = true,

            // Validate signing key (security)
            ValidateIssuerSigningKey = true,

            // Valid issuer from config
            ValidIssuer = jwtSection["Issuer"],

            // Valid audience from config
            ValidAudience = jwtSection["Audience"],

            // Secret key used to sign the token
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Secret"]!))
        };
    });

// Add Authorization (used with [Authorize] attribute)
builder.Services.AddAuthorization();


// ========================= RABBITMQ (MASSTRANSIT) =========================

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Configure RabbitMQ host
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            // Credentials
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });
    });
});


// ========================= BUILD APPLICATION =========================

var app = builder.Build();


// ========================= MIDDLEWARE PIPELINE =========================

// Global Exception Handling Middleware
// - Must be early in pipeline to catch all exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();

//////////////
app.UseSerilogRequestLogging();

// Enable Swagger only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Authentication Middleware
// - Validates JWT token
app.UseAuthentication();

// Authorization Middleware
// - Checks permissions ([Authorize])
app.UseAuthorization();


// Map controller endpoints
app.MapControllers();


// Run the application
app.Run();