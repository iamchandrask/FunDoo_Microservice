namespace Fundoo.NotesService.Application.Contracts;

public interface INotesCacheService
{
    Task<string?> GetAsync(Guid userId);
    Task SetAsync(Guid userId, string value, TimeSpan expiry);
    Task RemoveAsync(Guid userId);
}