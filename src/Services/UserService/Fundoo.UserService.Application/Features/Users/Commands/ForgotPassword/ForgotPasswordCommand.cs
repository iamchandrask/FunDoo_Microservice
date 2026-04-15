using Fundoo.UserService.Application.DTOs;
using MediatR;

namespace Fundoo.UserService.Application.Features.Users.Commands.ForgotPassword;

public record ForgotPasswordCommand(ForgotPasswordRequest Request) : IRequest<string>;