using BCrypt.Net;
using Fundoo.UserService.Application.Contracts;
using Fundoo.UserService.Domain.Entities;
using MediatR;
using Fundoo.UserService.Application.Features.Users.Commands.RegisterUser;

namespace Fundoo.UserService.Application.Features.Users.Commands.Register;

// CQRS Command Handler:
// Handles the RegisterUserCommand and performs the "WRITE" operation
// of creating a new user in the system.

// Responsibilities of this handler:
// 1) Validate if the user already exists (business rule).
// 2) Hash the password securely.
// 3) Create a new User entity.
// 4) Persist the user using repository pattern.
// 5) Return the created user's Id.

// MediatR Integration:
// - Implements IRequestHandler<RegisterUserCommand, Guid>
// - Receives RegisterUserCommand as input
// - Returns Guid (Id of the newly created user)

// This class belongs to the Application Layer:
// - Contains business logic (not infrastructure-specific code).
// - Uses abstraction (IUserRepository) instead of direct DB access.

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    // Repository abstraction for user-related database operations.
    // - Helps in maintaining loose coupling.
    // - Enables easy unit testing and mocking.
    private readonly IUserRepository _userRepository;

    // Constructor Injection:
    // - IUserRepository is injected via Dependency Injection.
    // - Ensures handler does not directly depend on implementation details.
    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Handle Method:
    // - This method is invoked by MediatR when RegisterUserCommand is sent.
    // - Contains the core logic for user registration.
    public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        // Step 1: Check if a user with the same email already exists.
        // - Email must be unique in the system.
        // - Prevents duplicate registrations.
        var existingUser = await _userRepository.GetByEmailAsync(command.Request.Email, cancellationToken);

        // If user already exists, throw an exception.
        // - This enforces a business rule.
        // - Can be handled globally using middleware/exception handling.
        if (existingUser is not null)
            throw new InvalidOperationException("User already exists with this email.");

        // Step 2: Create a new User entity.
        // - Map data from DTO (Request) to Domain Entity.
        // - Password is hashed before storing (security best practice).
        var user = new User
        {
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            Email = command.Request.Email,

            // Password Hashing:
            // - Converts plain text password into a secure hashed format.
            // - BCrypt automatically handles salting + hashing.
            // - Protects against password leaks and attacks.
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Request.Password)
        };

        // Step 3: Add the user to the repository.
        // - This typically stages the entity for insertion.
        await _userRepository.AddAsync(user, cancellationToken);

        // Step 4: Persist changes to the database.
        // - Commits the transaction.
        await _userRepository.SaveChangesAsync(cancellationToken);

        // Step 5: Return the Id of the newly created user.
        // - Useful for further operations or response handling.
        return user.Id;
    }
}