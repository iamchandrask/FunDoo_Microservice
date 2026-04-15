using Fundoo.NotesService.Application.DTOs;
using MediatR;

namespace Fundoo.NotesService.Application.Features.Notes.Commands.CreateNote;

public record CreateNoteCommand(Guid UserId, CreateNoteRequest Request) : IRequest<Guid>;