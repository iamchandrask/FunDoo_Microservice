using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.UserService.Domain.Entities
{
    // Domain Entity:
    // Represents a User in the system.
    // This class maps directly to the Users table in the database.
    // It contains all the core properties required to manage user data.
    // This entity is part of the Domain layer (DDD), meaning:
    // - It holds business data
    // - It should remain persistence-agnostic (no DB-specific logic here)
    // - It can be used across application layers

    public class User
    {
        // Unique identifier for the user.
        // - Using GUID ensures global uniqueness across distributed systems.
        // - Automatically generated when a new user is created.
        public Guid Id { get; set; } = Guid.NewGuid();

        // User's first name.
        // - Initialized with empty string to avoid null reference issues.
        public string FirstName { get; set; } = string.Empty;

        // User's last name.
        public string LastName { get; set; } = string.Empty;

        // User's email address.
        // - This should be unique in the system.
        // - Used for login and communication.
        public string Email { get; set; } = string.Empty;

        // Stores the hashed password of the user.
        // - NEVER store plain text passwords.
        // - Hashing ensures security (e.g., using BCrypt, SHA, etc.).
        public string PasswordHash { get; set; } = string.Empty;

        // Indicates whether the user's email has been verified.
        // - Used for authentication/authorization flows.
        // - Default is false until verification is completed.
        public bool IsEmailVerified { get; set; } = false;

        // Token used for password reset functionality.
        // - Generated when the user requests a password reset.
        // - Nullable because it is only present during reset flow.
        public string? ResetToken { get; set; }

        // Expiration time for the reset token.
        // - Ensures the token is valid only for a limited time.
        // - Nullable because it is only set when a reset token exists.
        public DateTime? ResetTokenExpiresAt { get; set; }

        // Timestamp when the user was created.
        // - Stored in UTC to maintain consistency across time zones.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Timestamp when the user was last updated.
        // - Should be updated whenever user data changes.
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}