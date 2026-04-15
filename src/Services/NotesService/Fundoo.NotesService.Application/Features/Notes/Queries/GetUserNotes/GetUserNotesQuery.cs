using Fundoo.NotesService.Application.DTOs;
using MediatR;

namespace Fundoo.NotesService.Application.Features.Notes.Queries.GetUserNotes;

public record GetUserNotesQuery(Guid UserId) : IRequest<List<NoteResponse>>;