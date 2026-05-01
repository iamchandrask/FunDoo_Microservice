using Fundoo.UserService.Application.Contracts;
using Fundoo.UserService.Application.DTOs;
using Fundoo.UserService.Application.Features.Users.Commands.Register;
using Fundoo.UserService.Application.Features.Users.Commands.RegisterUser;
using Fundoo.UserService.Domain.Entities;
using Moq;
using Xunit;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_User_When_Email_Does_Not_Exist()
    {
        var repositoryMock = new Mock<IUserRepository>();

        repositoryMock
            .Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new RegisterUserCommandHandler(repositoryMock.Object);

        var command = new RegisterUserCommand(
            new RegisterUserRequest("Test", "User", "test@example.com", "Password@123"));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}