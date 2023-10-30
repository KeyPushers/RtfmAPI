using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Album"/>.
/// </summary>
public class AlbumsRepository : IAlbumsRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных</param>.
    public AlbumsRepository(AppDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc cref="IAlbumsRepository.GetAlbumsAsync"/>
    public Task<List<Album>> GetAlbumsAsync()
    {
        return _context.Set<Album>().ToListAsync();
    }

    /// <inheritdoc cref="IAlbumsRepository.GetAlbumByIdAsync"/>
    public Task<Album?> GetAlbumByIdAsync(AlbumId albumId)
    {
        return _context.Set<Album>().FirstOrDefaultAsync(entity => entity.Id == albumId);
    }

    /// <inheritdoc cref="IAlbumsRepository.AddAsync"/>
    public async Task AddAsync(Album album)
    {
        await _context.AddAsync(album);
    }

}