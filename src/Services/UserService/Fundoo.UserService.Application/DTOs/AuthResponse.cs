using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.UserService.Application.DTOs
{
    // Data Transfer Object (DTO):
    // Represents the response returned after successful authentication (Login/Register).
    // This DTO is part of the Application layer and is used to transfer data
    // from the backend to the client (API response).

    // Why "record"?
    // - Records are immutable by default (good for response models).
    // - They provide built-in value equality.
    // - Ideal for lightweight data carriers in CQRS.

    // This response typically contains:
    // 1) JWT Token for authentication
    // 2) User email for identification
    // 3) Full name for display purposes

    public record AuthResponse(

        // JWT Token generated after successful authentication.
        // - Used by the client for subsequent authorized requests.
        // - Should be included in the Authorization header (Bearer token).
        string Token,

        // Email of the authenticated user.
        // - Acts as a unique identifier in most systems.
        string Email,

        // Full name of the user.
        // - Usually constructed from FirstName + LastName.
        // - Helpful for UI display (e.g., dashboard, profile).
        string FullName
    );
}