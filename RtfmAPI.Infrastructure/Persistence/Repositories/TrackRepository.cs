using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Aggregates.Tracks;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий музыкальных треков
/// </summary>
public class TrackRepository : ITrackRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий музыкальных треков
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public TrackRepository(AppDbContext context)
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
        var track = _context.Find<Track>(id);
        
        return Task.FromResult(track);
    }

    /// <inheritdoc/>
    public Task AddAsync(Track track)
    {
        _context.AddAsync(track);

        _context.SaveChanges();

        return Task.CompletedTask;
    }
}