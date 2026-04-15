using Fundoo.UserService.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Fundoo.UserService.Infrastructure.Persistence;

// DbContext:
// Represents the session with the database using Entity Framework Core.
// It is responsible for:
// - Managing entity objects during runtime
// - Translating LINQ queries into SQL
// - Tracking changes and persisting them to the database

// This class belongs to the Infrastructure Layer:
// - Contains database-related configurations
// - Implements persistence logic using EF Core

public class UserDbContext : DbContext
{
    // Constructor:
    // - Accepts DbContextOptions via Dependency Injection
    // - Options include connection string, provider (SQL Server), etc.
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    // DbSet<User>:
    // - Represents the Users table in the database
    // - Allows querying and saving instances of User entity
    // - EF Core maps this to a table automatically
    public DbSet<User> Users => Set<User>();


    // OnModelCreating:
    // - Used to configure entity mappings using Fluent API
    // - Overrides default EF Core conventions
    // - Executes when the model is being created

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuring the User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Maps entity to "Users" table in database
            entity.ToTable("Users");

            // Configures primary key
            entity.HasKey(x => x.Id);

            // Property Configurations:

            // FirstName:
            // - Required field
            // - Maximum length = 100 characters
            entity.Property(x => x.FirstName)
                  .HasMaxLength(100)
                  .IsRequired();

            // LastName:
            // - Required field
            // - Maximum length = 100 characters
            entity.Property(x => x.LastName)
                  .HasMaxLength(100)
                  .IsRequired();

            // Email:
            // - Required field
            // - Maximum length = 150 characters
            // - Should be unique (configured below)
            entity.Property(x => x.Email)
                  .HasMaxLength(150)
                  .IsRequired();

            // PasswordHash:
            // - Required field
            // - Stores hashed password (never plain text)
            entity.Property(x => x.PasswordHash)
                  .IsRequired();

            // Index Configuration:

            // Creates a unique index on Email column
            // - Ensures no duplicate users with same email
            // - Improves lookup performance during login
            entity.HasIndex(x => x.Email)
                  .IsUnique();
        });
    }
}