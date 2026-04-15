using Fundoo.UserService.Application.DTOs;
using MediatR;

namespace Fundoo.UserService.Application.Features.Users.Queries.Login;

// CQRS Query:
// Represents a "READ" operation in the system.
// This query is responsible for handling user login/authentication.

// Purpose of this query:
// - Carries the login request data (Email + Password).
// - Delegates authentication logic to a corresponding Query Handler.
// - Does NOT contain business logic itself.

// MediatR Integration:
// - Implements IRequest<AuthResponse>, which means:
//   -> This query will be handled by a handler implementing:
//        IRequestHandler<LoginUserQuery, AuthResponse>
//   -> The handler will return an AuthResponse DTO containing:
//        - JWT Token
//        - Email
//        - Full Name

// Difference from Command:
// - Query → Used for retrieving data (no state change).
// - Command → Used for modifying data (create/update/delete).

// Why use "record"?
// - Immutable (ensures request data cannot be modified).
// - Lightweight and concise syntax.
// - Ideal for CQRS request models.

public record LoginUserQuery(

    // The login request payload.
    // - Contains user credentials (Email and Password).
    // - Passed from API layer to Application layer.
    LoginRequest Request

) : IRequest<AuthResponse>; // Returns authentication response (token + user details)