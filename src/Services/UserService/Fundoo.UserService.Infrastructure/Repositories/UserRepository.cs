using Fundoo.UserService.Application.Contracts;
using Fundoo.UserService.Domain.Entities;
using Fundoo.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.UserService.Infrastructure.Repositories;

// Repository Implementation:
// Implements IUserRepository interface using Entity Framework Core.
// This class belongs to the Infrastructure layer and is responsible for
// actual database operations related to the User entity.

// Responsibilities:
// - Execute database queries using DbContext
// - Provide data access methods to Application layer
// - Keep data access logic separated from business logic

public class UserRepository : IUserRepository
{
    // DbContext instance used to interact with the database.
    // - Injected via Dependency Injection.
    // - Acts as a session for querying and saving data.
    private readonly UserDbContext _dbContext;

    // Constructor Injection:
    // - Receives UserDbContext from DI container.
    // - Promotes loose coupling and easier testing.
    public UserRepository(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Retrieves a user by email.
    // - Used in login and registration validation.
    // - Returns null if no user is found.

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        // FirstOrDefaultAsync:
        // - Returns the first matching user or null.
        // - cancellationToken ensures query can be cancelled if needed.
        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    // Retrieves a user by reset token.
    // - Used in password reset flow.
    // - Ensures the token belongs to a valid user.

    public async Task<User?> GetByResetTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.ResetToken == token, cancellationToken);
    }

    // Adds a new user to the database context.
    // - This does NOT immediately save to database.
    // - Changes are persisted only after calling SaveChangesAsync().

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        // AddAsync:
        // - Marks the entity as "Added" in EF Core change tracker.
        // - cancellationToken allows cancellation of the operation.
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    // Updates an existing user.
    // - Marks the entity as "Modified".
    // - Changes will be saved when SaveChangesAsync() is called.

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        // Update:
        // - Attaches entity (if not already tracked)
        // - Marks all properties as modified
        _dbContext.Users.Update(user);

        // No async operation here, so return completed task
        return Task.CompletedTask;
    }

    // Persists all pending changes to the database.
    // - Executes SQL INSERT/UPDATE/DELETE operations.
    // - Should be called after AddAsync/UpdateAsync.

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // SaveChangesAsync:
        // - Commits all tracked changes to database
        // - Uses cancellationToken for graceful cancellation
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}