using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Band"/>.
/// </summary>
public class BandsRepository : IBandsRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Band"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public BandsRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// <inheritdoc cref="IBandsRepository.GetBandsAsync"/>
    /// </summary>
    public Task<List<Band>> GetBandsAsync()
    {
        return _context.Set<Band>().ToListAsync();
    }

    /// <summary>
    /// <inheritdoc cref="IBandsRepository.GetBandsByAlbumIdAsync"/>
    /// </summary>
    public Task<List<Band>> GetBandsByAlbumIdAsync(AlbumId albumId)
    {
        return _context.Set<Band>()
            .Where(entity => entity.AlbumIds.Contains(albumId))
            .ToListAsync();
    }

    /// <summary>
    /// <inheritdoc cref="IBandsRepository.GetBandByIdAsync"/>
    /// </summary>
    public Task<Band?> GetBandByIdAsync(BandId bandId)
    {
        return _context.Set<Band>().FirstOrDefaultAsync(entity => entity.Id == bandId);
    }

    /// <summary>
    /// <inheritdoc cref="IBandsRepository.AddAsync"/>
    /// </summary>
    public async Task AddAsync(Band band)
    {
        await _context.AddAsync(band);
    }

    /// <summary>
    /// <inheritdoc cref="IBandsRepository.DeleteAsync"/>
    /// </summary>
    public Task<bool> DeleteAsync(Band band)
    {
        var deleteAction = _context.Remove(band);
        return Task.FromResult(deleteAction.State is EntityState.Deleted);
    }
}