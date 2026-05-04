using Fundoo.LabelService.Application.DTOs;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Queries.GetAllNotes;

public record GetAllNotesQuery(Guid UserId) : IRequest<List<NoteResponse>>;