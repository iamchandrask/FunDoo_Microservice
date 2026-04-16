using MassTransit;
using Shared.Contracts.Events;

namespace Fundoo.UserService.Infrastructure.Messaging;

// Event Publisher:
// Responsible for publishing events to the message broker (RabbitMQ via MassTransit).
// This class is used to notify other microservices when something important happens,
// such as a new user being registered.

// Purpose:
// - Enables communication between microservices (event-driven architecture)
// - Decouples services (UserService does not directly call other services)
// - Improves scalability and reliability

public class UserEventPublisher
{
    // IPublishEndpoint:
    // - Provided by MassTransit
    // - Used to publish events to the message broker (RabbitMQ)
    // - Automatically routes messages to subscribed consumers

    private readonly IPublishEndpoint _publishEndpoint;

    // Constructor Injection:
    // - IPublishEndpoint is injected via Dependency Injection
    // - Allows publishing messages without tightly coupling to RabbitMQ
    public UserEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    // Publishes UserRegisteredEvent:
    // - Triggered after a user is successfully registered
    // - Sends user details to other services (e.g., EmailService, NotificationService)

    public async Task PublishUserRegisteredAsync(
        Guid userId,
        string email,
        string firstName,
        string lastName)
    {
        // Create and publish event
        await _publishEndpoint.Publish(new UserRegisteredEvent(

            // Unique identifier of the user
            userId,

            // Email of the registered user
            email,

            // First name of the user
            firstName,

            // Last name of the user
            lastName,

            // Timestamp of when the event occurred (UTC for consistency)
            DateTime.UtcNow
        ));
    }
}