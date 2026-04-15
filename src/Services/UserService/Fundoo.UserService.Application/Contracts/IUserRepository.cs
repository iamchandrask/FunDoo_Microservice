using Fundoo.UserService.Domain.Entities;

namespace Fundoo.UserService.Application.Contracts;

// Repository Interface:
// Defines the contract for user-related data operations.
// This interface belongs to the Application layer and follows
// the Repository Pattern to abstract database access logic.

// Purpose:
// - Decouples business logic from data access (loose coupling).
// - Makes the application easier to test (mocking).
// - Allows switching database implementations without changing business logic.

// NOTE:
// - This interface does NOT contain implementation.
// - Implementation will be provided in Infrastructure layer (e.g., EF Core).

public interface IUserRepository
{
    // Retrieves a user by their email address.
    // - Used during login and registration validation.
    // - Returns null if user is not found.

    // Parameters:
    // email → Email of the user to search.
    // cancellationToken → Allows operation to be cancelled if request is aborted.

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);


    // Retrieves a user using a password reset token.
    // - Used in "Forgot Password" / "Reset Password" flow.
    // - Ensures token is valid and belongs to a user.

    // Parameters:
    // token → Reset token sent to user via email.
    // cancellationToken → Supports cancellation of DB operation.

    Task<User?> GetByResetTokenAsync(string token, CancellationToken cancellationToken = default);


    // Adds a new user to the database.
    // - Typically used during registration.
    // - Does NOT immediately persist changes (depends on implementation).

    // Parameters:
    // user → User entity to be added.
    // cancellationToken → Supports cancelling the operation.

    Task AddAsync(User user, CancellationToken cancellationToken = default);


    // Updates an existing user in the database.
    // - Used when modifying user details (e.g., password reset, profile update).

    // Parameters:
    // user → Updated user entity.
    // cancellationToken → Supports cancelling the operation.

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);


    // Persists all pending changes to the database.
    // - Acts like committing a transaction.
    // - Usually implemented using DbContext.SaveChangesAsync() in EF Core.

    // Parameters:
    // cancellationToken → Allows cancellation of the save operation.

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}