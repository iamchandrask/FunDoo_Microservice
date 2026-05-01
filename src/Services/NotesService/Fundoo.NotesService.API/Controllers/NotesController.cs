using System.IdentityModel.Tokens.Jwt;
using Fundoo.NotesService.Application.DTOs;
using Fundoo.NotesService.Application.Features.Notes.Commands.CreateNote;
using Fundoo.NotesService.Application.Features.Notes.Commands.UpdateNote;
using Fundoo.NotesService.Application.Features.Notes.Queries.GetUserNotes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.NotesService.API.Controllers;

[ApiController]
[Route("api/notes")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(JwtRegisteredClaimNames.Sub)
                    ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (claim == null)
            throw new UnauthorizedAccessException("User ID claim not found in token");

        if (!Guid.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid User ID format");

        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request)
    {
        var result = await _mediator.Send(new CreateNoteCommand(GetUserId(), request));
        return Ok(new { Message = "Note created successfully", NoteId = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotes()
    {
        var result = await _mediator.Send(new GetUserNotesQuery(GetUserId()));
        return Ok(result);
    }

    [HttpPut("{noteId:guid}")]
    public async Task<IActionResult> Update(Guid noteId, [FromBody] UpdateNoteRequest request)
    {
        var result = await _mediator.Send(new UpdateNoteCommand(noteId, GetUserId(), request));
        return Ok(new { Message = "Note updated successfully", Success = result });
    }
}