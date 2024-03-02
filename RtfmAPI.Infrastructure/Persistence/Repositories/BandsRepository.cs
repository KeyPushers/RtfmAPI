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
    private readonly DbSet<Band> _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Band"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public BandsRepository(AppDbContext context)
    {
        _context = context.Set<Band>();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandsAsync"/>
    public Task<List<Band>> GetBandsAsync()
    {
        return _context.ToListAsync();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandsByAlbumIdAsync"/>
    public Task<List<Band>> GetBandsByAlbumIdAsync(AlbumId albumId)
    {
        return _context
            .Where(entity => entity.AlbumIds.Any(id => ReferenceEquals(id, albumId)))
            .ToListAsync();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandByIdAsync"/>
    public Task<Band?> GetBandByIdAsync(BandId bandId)
    {
        return _context.FirstOrDefaultAsync(entity => ReferenceEquals(entity.Id, bandId));
    }

    /// <inheritdoc cref="IBandsRepository.AddAsync"/>
    public async Task AddAsync(Band band)
    {
        await _context.AddAsync(band);
    }

    /// <inheritdoc cref="IBandsRepository.UpdateAsync"/>
    public Task UpdateAsync(Band band)
    {
        _context.Update(band);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IBandsRepository.DeleteAsync"/>
    public Task<bool> DeleteAsync(Band band)
    {
        var deleteAction = _context.Remove(band);
        return Task.FromResult(deleteAction.State is EntityState.Deleted);
    }
}