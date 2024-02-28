using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Dao.Tracks;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Track"/>.
/// </summary>
public class TracksRepository : ITracksRepository
{
    private readonly IMapper _mapper;
    private readonly DbSet<TrackDao> _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Track"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="mapper">Маппер.</param>
    public TracksRepository(AppDbContext context, IMapper mapper)
    {
        _context = context.Set<TrackDao>();
        _mapper = mapper;
    }

    /// <inheritdoc cref="ITracksRepository.GetTracksAsync"/>
    public Task<List<Track>> GetTracksAsync()
    {
        return _context.Select(entity => _mapper.Map<Track>(entity)).ToListAsync();
    }

    /// <inheritdoc cref="ITracksRepository.GetTrackByIdAsync"/>
    public async Task<Track?> GetTrackByIdAsync(TrackId trackId)
    {
        var value = await _context.FirstOrDefaultAsync(entity => entity.Id == trackId.Value);
        return value is null ? null : _mapper.Map<Track>(value);
    }

    /// <inheritdoc cref="ITracksRepository.AddAsync"/>
    public async Task AddAsync(Track track)
    {
        var trackDao = _mapper.Map<TrackDao>(track);
        await _context.AddAsync(trackDao);
    }

    /// <inheritdoc cref="ITracksRepository.DeleteAsync"/>
    public Task<bool> DeleteAsync(Track track)
    {
        var trackDao = _mapper.Map<TrackDao>(track);
        var removeResult = _context.Remove(trackDao);
        return Task.FromResult(removeResult.State is EntityState.Deleted);
    }
}