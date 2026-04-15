using Fundoo.NotesService.Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace Fundoo.NotesService.Infrastructure.Caching;

public class NotesCacheService : INotesCacheService
{
    private readonly IDistributedCache _cache;

    public NotesCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    private static string BuildKey(Guid userId) => $"fundoo:notes:{userId}";

    public async Task<string?> GetAsync(Guid userId)
    {
        return await _cache.GetStringAsync(BuildKey(userId));
    }

    public async Task SetAsync(Guid userId, string value, TimeSpan expiry)
    {
        await _cache.SetStringAsync(
            BuildKey(userId),
            value,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            });
    }

    public async Task RemoveAsync(Guid userId)
    {
        await _cache.RemoveAsync(BuildKey(userId));
    }
}