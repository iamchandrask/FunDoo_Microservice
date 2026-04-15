using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fundoo.UserService.Application.DTOs;

namespace Fundoo.UserService.Application.Features.Users.Commands.RegisterUser
{
    // CQRS Command:
    // Represents a "WRITE" operation in the system (i.e., it modifies state).
    // This command is responsible for handling user registration.

    // Purpose of this command:
    // - Encapsulates all the data required to register a new user.
    // - Delegates the actual business logic to a corresponding Command Handler.
    // - Keeps the API/controller thin and clean.

    // MediatR Integration:
    // - Implements IRequest<Guid>, which means:
    //   -> This command will be handled by a handler implementing:
    //        IRequestHandler<RegisterUserCommand, Guid>
    //   -> The handler will return a Guid (typically the newly created User Id).

    // Why use "record"?
    // - Immutable by default (good practice for commands).
    // - Ensures data consistency (command should not change after creation).
    // - Lightweight and concise syntax.

    // Flow:
    // Controller → RegisterUserCommand → MediatR → Handler → Database

    public record RegisterUserCommand(

        // The request DTO containing user registration details.
        // - Includes FirstName, LastName, Email, and Password.
        // - Acts as the payload for creating a new user.
        // - Passed from API layer into the Application layer.
        RegisterUserRequest Request

    ) : IRequest<Guid>; // Returns the unique identifier (Id) of the created user
}