using Fundoo.ReminderService.Application.Contracts;
using Fundoo.ReminderService.Application.Features.Reminders.Commands.AddReminder;
using Fundoo.ReminderService.Infrastructure.Persistence;
using Fundoo.ReminderService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;



// Register DbContext with SQL Server
builder.Services.AddDbContext<ReminderDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register repository for DI
// This allows MediatR handlers to use ICollaboratorRepository abstraction
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();



// Register MediatR and scan Application assembly for handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(
        AddReminderCommand
    ).Assembly));

builder.Services.AddControllers();

// Swagger is useful for testing APIs independently
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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