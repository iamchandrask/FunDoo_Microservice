using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

const string AuthProviderKey = "Bearer";

var jwtSection = builder.Configuration.GetSection("JwtSettings");

builder.Services
    .AddAuthentication()
    .AddJwtBearer(AuthProviderKey, options =>
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
                Encoding.UTF8.GetBytes(jwtSection["Secret"]!))
        };
    });

// 🔥 ADD THIS
builder.Services.AddAuthorization();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization(); // 🔥 REQUIRED

await app.UseOcelot();

app.MapHealthChecks("/health");

app.Run();