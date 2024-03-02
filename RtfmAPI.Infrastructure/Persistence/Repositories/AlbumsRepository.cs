using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Album"/>.
/// </summary>
public class AlbumsRepository : IAlbumsRepository
{
    private readonly DbSet<Album> _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public AlbumsRepository(AppDbContext context)
    {
        _context = context.Set<Album>();
    }

    /// <inheritdoc cref="IAlbumsRepository.GetAlbumsAsync"/>
    public Task<List<Album>> GetAlbumsAsync()
    {
        return _context.ToListAsync();
    }

    /// <inheritdoc cref="IAlbumsRepository.GetAlbumByIdAsync"/>
    public Task<Album?> GetAlbumByIdAsync(AlbumId albumId)
    {
        return _context.FirstOrDefaultAsync(entity => ReferenceEquals(entity.Id, albumId));
    }

    /// <inheritdoc cref="IAlbumsRepository.GetAlbumsByTrackIdAsync"/>
    public Task<List<Album>> GetAlbumsByTrackIdAsync(TrackId trackId)
    {
        return _context.Where(entity => entity.TrackIds.Any(id => ReferenceEquals(id, trackId))).ToListAsync();
    }

    /// <inheritdoc cref="IAlbumsRepository.AddAsync"/>
    public async Task AddAsync(Album album)
    {
        await _context.AddAsync(album);
    }

    /// <inheritdoc cref="IAlbumsRepository.UpdateAsync"/>
    public Task UpdateAsync(Album album)
    {
        _context.Update(album);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IAlbumsRepository.DeleteAlbumAsync"/>
    public Task<bool> DeleteAlbumAsync(Album album)
    {
        var removeResult = _context.Remove(album);
        return Task.FromResult(removeResult.State is EntityState.Deleted);
    }
}