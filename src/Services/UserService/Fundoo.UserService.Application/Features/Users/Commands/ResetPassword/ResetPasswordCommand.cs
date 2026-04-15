using Fundoo.UserService.Application.DTOs;
using MediatR;

namespace Fundoo.UserService.Application.Features.Users.Commands.ResetPassword;

public record ResetPasswordCommand(ResetPasswordRequest Request) : IRequest<bool>;