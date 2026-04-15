using Fundoo.NotesService.Application.DTOs;
using MediatR;

namespace Fundoo.NotesService.Application.Features.Notes.Commands.UpdateNote;

public record UpdateNoteCommand(Guid NoteId, Guid UserId, UpdateNoteRequest Request) : IRequest<bool>;