using Fundoo.NotesService.Domain.Entities;
using Fundoo.NotesService.Infrastructure.Persistence;
using MassTransit;
using Shared.Contracts.Events;

namespace Fundoo.NotesService.API.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly NotesDbContext _dbContext;

    public UserRegisteredConsumer(NotesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var note = new Note
        {
            UserId = context.Message.UserId,
            Title = "Welcome to Fundoo Notes",
            Description = $"Hello {context.Message.FirstName}, your account is ready.",
            Color = "#E8F5E9"
        };

        await _dbContext.Notes.AddAsync(note);
        await _dbContext.SaveChangesAsync();
    }
}