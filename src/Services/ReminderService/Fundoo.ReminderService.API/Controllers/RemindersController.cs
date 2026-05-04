using Fundoo.ReminderService.Application.DTOs;
using Fundoo.ReminderService.Application.Features.Reminders.Commands.AddReminder;
using Fundoo.ReminderService.Application.Features.Reminders.Commands.UpdateReminder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.ReminderService.API.Controllers;

[ApiController]
[Route("api/reminders")]
[Authorize]
public class RemindersController : ControllerBase
{
    private readonly IMediator _mediator;

    public RemindersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddReminderRequest request)
    {
        var result = await _mediator.Send(new AddReminderCommand(request));
        return Ok(new { Message = "Reminder added successfully", ReminderId = result });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateReminderRequest request)
    {
        var result = await _mediator.Send(new UpdateReminderCommand(id, request));
        return Ok(new { Message = "Reminder updated successfully", Success = result });
    }
}