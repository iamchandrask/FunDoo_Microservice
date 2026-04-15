using System.Text.Json;
using Fundoo.NotesService.Application.Contracts;
using Fundoo.NotesService.Application.DTOs;
using MediatR;

namespace Fundoo.NotesService.Application.Features.Notes.Queries.GetUserNotes;

public class GetUserNotesQueryHandler : IRequestHandler<GetUserNotesQuery, List<NoteResponse>>
{
    private readonly INoteRepository _repository;
    private readonly INotesCacheService _cacheService;

    public GetUserNotesQueryHandler(INoteRepository repository, INotesCacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<List<NoteResponse>> Handle(GetUserNotesQuery query, CancellationToken cancellationToken)
    {
        var cached = await _cacheService.GetAsync(query.UserId);

        if (!string.IsNullOrWhiteSpace(cached))
        {
            var cachedNotes = JsonSerializer.Deserialize<List<NoteResponse>>(cached);
            if (cachedNotes is not null)
                return cachedNotes;
        }

        var notes = await _repository.GetByUserIdAsync(query.UserId, cancellationToken);

        var result = notes.Select(x => new NoteResponse(
            x.Id,
            x.UserId,
            x.Title,
            x.Description,
            x.Color,
            x.IsPinned,
            x.IsArchived,
            x.IsTrashed,
            x.CreatedAt,
            x.UpdatedAt)).ToList();

        var serialized = JsonSerializer.Serialize(result);
        await _cacheService.SetAsync(query.UserId, serialized, TimeSpan.FromMinutes(10));

        return result;
    }
}