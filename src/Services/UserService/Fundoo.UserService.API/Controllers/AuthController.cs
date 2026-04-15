using Fundoo.UserService.Application.DTOs;
using Fundoo.UserService.Application.Features.Users.Commands.ForgotPassword;
using Fundoo.UserService.Application.Features.Users.Commands.Register;
using Fundoo.UserService.Application.Features.Users.Commands.RegisterUser;
using Fundoo.UserService.Application.Features.Users.Commands.ResetPassword;
using Fundoo.UserService.Application.Features.Users.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.UserService.API.Controllers;

// API Controller:
// Handles HTTP requests related to authentication (Register, Login, Forgot Password, Reset Password)

// Attributes:
// [ApiController]
// - Enables automatic model validation
// - Improves API behavior (binding, error responses)

// [Route("api/auth")]
// - Base route for all endpoints in this controller
// - Example: api/auth/register, api/auth/login

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    // IMediator:
    // - Central component of MediatR library
    // - Used to send Commands/Queries to their respective handlers
    // - Decouples controller from business logic

    private readonly IMediator _mediator;

    // Constructor Injection:
    // - IMediator is injected via Dependency Injection
    // - Controller does not depend on services directly
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ========================= REGISTER =========================

    // Endpoint: POST /api/auth/register
    // Purpose:
    // - Registers a new user in the system

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        // 🔹 [FromBody]:
        // - Binds incoming JSON request to RegisterUserRequest DTO

        // Send RegisterUserCommand to MediatR
        // - Command Handler will:
        //   ✔ Validate user
        //   ✔ Hash password
        //   ✔ Save user to DB
        var userId = await _mediator.Send(new RegisterUserCommand(request));

        // Return success response
        return Ok(new
        {
            Message = "User registered successfully",
            UserId = userId
        });
    }

    // ========================= LOGIN =========================

    // Endpoint: POST /api/auth/login
    // Purpose:
    // - Authenticates user and returns JWT token

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Send LoginUserQuery to MediatR
        // - Query Handler will:
        //   ✔ Validate credentials
        //   ✔ Verify password using BCrypt
        //   ✔ Generate JWT token
        var response = await _mediator.Send(new LoginUserQuery(request));

        // Return AuthResponse (Token + Email + FullName)
        return Ok(response);
    }

    // ========================= FORGOT PASSWORD =========================

    // Endpoint: POST /api/auth/forgot-password
    // Purpose:
    // - Generates a password reset token

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        // Send ForgotPasswordCommand to MediatR
        // - Handler will:
        //   ✔ Validate email
        //   ✔ Generate reset token
        //   ✔ Save token in DB
        //   ✔ (Optionally send email)

        var token = await _mediator.Send(new ForgotPasswordCommand(request));

        // Return token (for testing; in real apps, send via email)
        return Ok(new
        {
            Message = "Reset token generated",
            ResetToken = token
        });
    }

    // ========================= RESET PASSWORD =========================

    // Endpoint: POST /api/auth/reset-password
    // Purpose:
    // - Resets user password using token

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        // Send ResetPasswordCommand to MediatR
        // - Handler will:
        //   ✔ Validate reset token
        //   ✔ Check expiration
        //   ✔ Hash new password
        //   ✔ Update user password

        var result = await _mediator.Send(new ResetPasswordCommand(request));

        // Return success response
        return Ok(new
        {
            Message = "Password reset successful",
            Success = result
        });
    }
}