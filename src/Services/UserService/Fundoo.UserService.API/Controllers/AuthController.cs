using Fundoo.UserService.Application.DTOs;
using Fundoo.UserService.Application.Features.Users.Commands.ForgotPassword;
using Fundoo.UserService.Application.Features.Users.Commands.Register;
using Fundoo.UserService.Application.Features.Users.Commands.RegisterUser;
using Fundoo.UserService.Application.Features.Users.Commands.ResetPassword;
using Fundoo.UserService.Application.Features.Users.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.UserService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var userId = await _mediator.Send(new RegisterUserCommand(request));
        return Ok(new { Message = "User registered successfully", UserId = userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _mediator.Send(new LoginUserQuery(request));
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var token = await _mediator.Send(new ForgotPasswordCommand(request));
        return Ok(new { Message = "Reset token generated", ResetToken = token });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _mediator.Send(new ResetPasswordCommand(request));
        return Ok(new { Message = "Password reset successful", Success = result });
    }
}