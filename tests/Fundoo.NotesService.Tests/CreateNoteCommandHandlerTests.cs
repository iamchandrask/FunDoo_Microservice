using Fundoo.NotesService.Application.Contracts;
using Fundoo.NotesService.Application.DTOs;
using Fundoo.NotesService.Application.Features.Notes.Commands.CreateNote;
using Fundoo.NotesService.Domain.Entities;
using Moq;
using Xunit;

namespace Fundoo.NotesService.Tests
{
    public class CreateNoteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_Note_And_Clear_Cache()
        {
            var repositoryMock = new Mock<INoteRepository>();
            var cacheMock = new Mock<INotesCacheService>();

            var command = new CreateNoteCommand(
                Guid.NewGuid(),
                new CreateNoteRequest("Architecture", "Complete Fundoo project", "#FFFFFF"));

            var handler = new CreateNoteCommandHandler(repositoryMock.Object, cacheMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);
            repositoryMock.Verify(x => x.AddAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            cacheMock.Verify(x => x.RemoveAsync(command.UserId), Times.Once);
        }
    }
}
