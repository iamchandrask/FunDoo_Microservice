using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.UserService.Application.DTOs
{
    // Data Transfer Object (DTO):
    // Represents the request payload for user registration.
    // This DTO is used to capture input data from the client (API request)
    // when a new user signs up.

    // Why "record"?
    // - Provides immutability (data cannot be changed after creation).
    // - Lightweight and ideal for request/response models in CQRS.
    // - Supports value-based equality.

    // This DTO will typically be passed to:
    // - A Command (e.g., RegisterUserCommand)
    // - Then handled by a Command Handler using MediatR

    public record RegisterUserRequest(

        // User's first name.
        // - Required for personal identification.
        string FirstName,

        // User's last name.
        string LastName,

        // User's email address.
        // - Must be unique in the system.
        // - Used for login and communication.
        string Email,

        // User's password (plain text at input stage).
        // - This should NEVER be stored directly.
        // - It must be hashed (e.g., BCrypt) before saving to the database.
        string Password
    );
}