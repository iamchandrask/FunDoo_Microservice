using System.Net.Http.Json;
using Fundoo.LabelService.Application.Contracts;
using Fundoo.LabelService.Application.DTOs;

namespace Fundoo.LabelService.Infrastructure.Services;

public class NoteServiceClient : INoteServiceClient
{
    private readonly HttpClient _httpClient;

    public NoteServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NoteResponse>> GetAllNotesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<List<NoteResponse>>(
            $"api/notes/user/{userId}",
            cancellationToken);

        return response ?? new List<NoteResponse>();
    }
}