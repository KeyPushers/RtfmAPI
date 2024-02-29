using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Dao.Dao.Album;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Album"/>.
/// </summary>
public class AlbumsRepository : IAlbumsRepository
{
    private readonly IMapper _mapper;
    private readonly DbSet<AlbumDao> _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="mapper">Маппер.</param>
    public AlbumsRepository(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context.Set<AlbumDao>();
    }

    /// <inheritdoc cref="IAlbumsRepository.GetAlbumsAsync"/>
    public Task<List<Album>> GetAlbumsAsync()
    {
        return _context.Select(entity => _mapper.Map<Album>(entity)).ToListAsync();
    }

    /// <inheritdoc cref="IAlbumsRepository.GetAlbumByIdAsync"/>
    public async Task<Album?> GetAlbumByIdAsync(AlbumId albumId)
    {
        var albumDao = await _context.FirstOrDefaultAsync(entity => entity.Id == albumId.Value);
        return albumDao is null ? null : _mapper.Map<Album>(albumDao);
    }

    /// <inheritdoc cref="IAlbumsRepository.AddAsync"/>
    public async Task AddAsync(Album album)
    {
        var albumDao = _mapper.Map<AlbumDao>(album);
        await _context.AddAsync(albumDao);
    }

    /// <inheritdoc cref="IAlbumsRepository.UpdateAsync"/>
    public Task UpdateAsync(Album album)
    {
        var albumDao = _mapper.Map<AlbumDao>(album);
        _context.Update(albumDao);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IAlbumsRepository.DeleteAlbumAsync"/>
    public Task<bool> DeleteAlbumAsync(Album album)
    {
        var albumDao = _mapper.Map<AlbumDao>(album);
        var removeResult = _context.Remove(albumDao);
        return Task.FromResult(removeResult.State is EntityState.Deleted);
    }
}