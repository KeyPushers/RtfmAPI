using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.Repository;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий музыкальных треков
/// </summary>
public class TracksRepository : ITracksRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий музыкальных треков
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public TracksRepository(AppDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc/>
    public Task<List<Track>> GetTracksAsync()
    {
        return _context.Set<Track>().ToListAsync();
    }

    /// <inheritdoc/>
    public Task<Track?> GetTrackByIdAsync(Guid id)
    {
        return _context.Set<Track>().FirstOrDefaultAsync(entity => entity.Id.Value == id);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Track track)
    {
        await _context.AddAsync(track).ConfigureAwait(false);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}