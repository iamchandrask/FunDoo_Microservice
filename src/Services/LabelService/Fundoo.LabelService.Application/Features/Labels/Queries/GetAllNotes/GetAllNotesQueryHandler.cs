using Fundoo.LabelService.Application.Contracts;
using Fundoo.LabelService.Application.DTOs;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Queries.GetAllNotes;

public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, List<NoteResponse>>
{
    private readonly INoteServiceClient _noteServiceClient;

    public GetAllNotesQueryHandler(INoteServiceClient noteServiceClient)
    {
        _noteServiceClient = noteServiceClient;
    }

    public async Task<List<NoteResponse>> Handle(GetAllNotesQuery query, CancellationToken cancellationToken)
    {
        return await _noteServiceClient.GetAllNotesAsync(query.UserId, cancellationToken);
    }
}