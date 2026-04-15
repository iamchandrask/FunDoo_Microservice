using BCrypt.Net;
using Fundoo.UserService.Application.Contracts;
using Fundoo.UserService.Application.DTOs;
using MediatR;

namespace Fundoo.UserService.Application.Features.Users.Queries.Login;

// CQRS Query Handler:
// Handles the LoginUserQuery and performs the "READ" operation
// of authenticating a user and returning authentication details.

// Responsibilities of this handler:
// 1) Fetch user by email from the database.
// 2) Verify the password using BCrypt.
// 3) Generate a JWT token upon successful authentication.
// 4) Return an AuthResponse DTO to the client.

// MediatR Integration:
// - Implements IRequestHandler<LoginUserQuery, AuthResponse>
// - Input  → LoginUserQuery (contains login credentials)
// - Output → AuthResponse (token + user details)

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResponse>
{
    // Repository abstraction for accessing user data.
    // - Used to fetch user details from the database.
    private readonly IUserRepository _userRepository;

    // Service responsible for generating JWT tokens.
    // - Encapsulates token creation logic (claims, expiry, signing key, etc.)
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    // Constructor Injection:
    // - Dependencies are injected via DI container.
    // - Promotes loose coupling and testability.
    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    // Handle Method:
    // - This method is called by MediatR when LoginUserQuery is sent.
    // - Contains the core authentication logic.

    public async Task<AuthResponse> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        // 🔹 Parameter: query
        // - Contains LoginRequest DTO.
        // - Includes Email and Password entered by the user.

        // 🔹 Parameter: cancellationToken
        // - Used to cancel the operation if the client disconnects
        //   or request is aborted.
        // - Helps in improving performance and avoiding unnecessary work.

        // Step 1: Fetch user from database using email.
        // - cancellationToken is passed so DB call can be cancelled if needed.
        var user = await _userRepository.GetByEmailAsync(query.Request.Email, cancellationToken);

        // Step 2: Validate user and verify password.
        // - BCrypt.Verify compares plain password with hashed password.
        // - Prevents storing or comparing plain text passwords.
        if (user is null || !BCrypt.Net.BCrypt.Verify(query.Request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        // Step 3: Generate JWT token.
        // - Token contains user claims (Id, Email, Roles, etc.)
        // - Used for authentication in future requests.
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Step 4: Return response DTO.
        // - Contains token + basic user info.
        return new AuthResponse(
            token,
            user.Email,
            $"{user.FirstName} {user.LastName}");
    }
}