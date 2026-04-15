using MassTransit;
using Shared.Contracts.Events;

namespace Fundoo.UserService.Infrastructure.Messaging;

public class UserEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public UserEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishUserRegisteredAsync(Guid userId, string email, string firstName, string lastName)
    {
        await _publishEndpoint.Publish(new UserRegisteredEvent(
            userId,
            email,
            firstName,
            lastName,
            DateTime.UtcNow));
    }
}