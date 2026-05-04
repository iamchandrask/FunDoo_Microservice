using Fundoo.LabelService.Application.DTOs;

namespace Fundoo.LabelService.Application.Contracts;

public interface INoteServiceClient
{
    Task<List<NoteResponse>> GetAllNotesAsync(Guid userId, CancellationToken cancellationToken);
}