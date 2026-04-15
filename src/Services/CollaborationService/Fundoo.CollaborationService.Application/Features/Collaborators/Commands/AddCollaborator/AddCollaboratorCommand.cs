using Fundoo.CollaborationService.Application.DTOs;
using MediatR;

namespace Fundoo.CollaborationService.Application.Features.Collaborators.Commands.AddCollaborator;

public record AddCollaboratorCommand(Guid OwnerUserId, AddCollaboratorRequest Request) : IRequest<Guid>;