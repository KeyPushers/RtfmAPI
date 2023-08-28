using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.Repository;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий альбомов
/// </summary>
public class AlbumsRepository : IAlbumsRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий альбомов
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public AlbumsRepository(AppDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc/>
    public Task<List<Album>> GetAlbumsAsync()
    {
        return _context.Set<Album>().ToListAsync();
    }

    /// <inheritdoc/>
    public Task<Album?> GetAlbumByIdAsync(Guid id)
    {
        return _context.Set<Album>().FirstOrDefaultAsync(entity => entity.Id.Value == id);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Album track)
    {
        await _context.AddAsync(track).ConfigureAwait(false);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}