using Fundoo.LabelService.Application.DTOs;
using Fundoo.LabelService.Application.Features.Labels.Commands.AddLabelToNote;
using Fundoo.LabelService.Application.Features.Labels.Commands.CreateLabel;
using Fundoo.LabelService.Application.Features.Labels.Commands.RemoveLabelFromNote;
using Fundoo.LabelService.Application.Features.Labels.Queries.GetAllNotes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Fundoo.LabelService.API.Controllers;

[ApiController]
[Route("api/labels")]
[Authorize]
public class LabelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LabelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelRequest request)
    {
        var result = await _mediator.Send(new CreateLabelCommand(GetUserId(), request));
        return Ok(new { Message = "Label created successfully", LabelId = result });
    }

    [HttpPost("add-to-note")]
    public async Task<IActionResult> AddToNote(AddLabelToNoteRequest request)
    {
        var result = await _mediator.Send(new AddLabelToNoteCommand(request));
        return Ok(new { Message = "Label added to note", Success = result });
    }

    [HttpDelete("remove-from-note")]
    public async Task<IActionResult> RemoveFromNote(RemoveLabelFromNoteRequest request)
    {
        var result = await _mediator.Send(new RemoveLabelFromNoteCommand(request));
        return Ok(new { Message = "Label removed from note", Success = result });
    }
    [HttpGet("notes/{userId:guid}")]
    public async Task<IActionResult> GetAllNotes(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllNotesQuery(userId), cancellationToken);
        return Ok(result);
    }
}