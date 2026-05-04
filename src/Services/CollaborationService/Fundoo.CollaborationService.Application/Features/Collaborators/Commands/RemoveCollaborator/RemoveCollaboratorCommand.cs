using Fundoo.CollaborationService.Application.DTOs;
using MediatR;

namespace Fundoo.CollaborationService.Application.Features.Collaborators.Commands.RemoveCollaborator;

public record RemoveCollaboratorCommand(RemoveCollaboratorRequest Request) : IRequest<bool>;