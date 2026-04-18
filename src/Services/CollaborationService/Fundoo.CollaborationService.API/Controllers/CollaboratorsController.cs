using System.IdentityModel.Tokens.Jwt;
using Fundoo.CollaborationService.Application.DTOs;
using Fundoo.CollaborationService.Application.Features.Collaborators.Commands.AddCollaborator;
using Fundoo.CollaborationService.Application.Features.Collaborators.Commands.RemoveCollaborator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.CollaborationService.API.Controllers;

[ApiController]
[Route("api/collaborators")]
[Authorize]
public class CollaboratorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollaboratorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    [HttpPost]
    public async Task<IActionResult> Add(AddCollaboratorRequest request)
    {
        var result = await _mediator.Send(new AddCollaboratorCommand(GetUserId(), request));
        return Ok(new { Message = "Collaborator added successfully", CollaboratorId = result });
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> Remove(RemoveCollaboratorRequest request)
    {
        var result = await _mediator.Send(new RemoveCollaboratorCommand(request));
        return Ok(new { Message = "Collaborator removed successfully", Success = result });
    }
}