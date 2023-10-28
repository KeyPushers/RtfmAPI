using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Repository;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Track"/>.
/// </summary>
public class TracksRepository : ITracksRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Track"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TracksRepository(AppDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc cref="ITracksRepository.GetTracksAsync"/>
    public Task<List<Track>> GetTracksAsync()
    {
        return _context.Set<Track>().ToListAsync();
    }
    
    /// <inheritdoc cref="ITracksRepository.GetTrackByIdAsync"/>
    public Task<Track?> GetTrackByIdAsync(TrackId trackId)
    {
        return _context.Set<Track>().FirstOrDefaultAsync(entity => entity.Id == trackId);
    }

    /// <inheritdoc cref="ITracksRepository.AddAsync"/>
    public async Task AddAsync(Track track)
    {
        await _context.AddAsync(track);
    }
}