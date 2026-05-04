using System.Text;
using Fundoo.CollaborationService.Application.Contracts;
using Fundoo.CollaborationService.Infrastructure.Persistence;
using Fundoo.CollaborationService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region ?? Configuration Setup

// Load configuration from appsettings.json (default behavior already included)
// You can also add environment-specific configs if needed.

var configuration = builder.Configuration;

#endregion

#region ??? Database Configuration (EF Core)

// Register DbContext with SQL Server
builder.Services.AddDbContext<CollaborationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

#endregion

#region ?? Dependency Injection (Repository Layer)

// Register repository for DI
// This allows MediatR handlers to use ICollaboratorRepository abstraction
builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();

#endregion

#region ?? MediatR Configuration (CQRS)

// Register MediatR and scan Application assembly for handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(
        Fundoo.CollaborationService.Application.Features.Collaborators.Commands.AddCollaborator.AddCollaboratorCommand
    ).Assembly));

#endregion

#region ?? JWT Authentication Setup

// This must match your API Gateway and other services
var jwtSection = configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Token validation rules (VERY IMPORTANT for security)
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,              // Validate token issuer
            ValidateAudience = true,            // Validate audience
            ValidateLifetime = true,            // Reject expired tokens
            ValidateIssuerSigningKey = true,    // Ensure token is signed correctly

            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],

            // Secret key used to sign JWT (must match auth service)
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Secret"]!))
        };
    });

#endregion

#region ?? Controllers + Swagger

builder.Services.AddControllers();

// Swagger is useful for testing APIs independently
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region ?? Middleware Pipeline

// Enable Swagger (IMPORTANT: enable always for Docker debugging)
app.UseSwagger();
app.UseSwaggerUI();

// Authentication MUST come before Authorization
app.UseAuthentication();

// Enforces [Authorize] attribute in controllers
app.UseAuthorization();

// Map controller routes
app.MapControllers();

#endregion

#region ?? Database Migration (Optional but useful)

// Auto-apply migrations on startup (optional in dev)
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<CollaborationDbContext>();
//    db.Database.Migrate();
//}

#endregion

app.Run();