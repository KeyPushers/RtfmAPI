using Microsoft.EntityFrameworkCore;
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
    
    /// <inheritdoc cref="IBandsRepository.GetBandsAsync"/>
    public Task<List<Band>> GetBandsAsync()
    {
        return _context.Set<Band>().ToListAsync();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandByIdAsync"/>
    public Task<Band?> GetBandByIdAsync(BandId bandId)
    {
        return _context.Set<Band>().FirstOrDefaultAsync(entity => entity.Id == bandId);
    }

    /// <inheritdoc cref="IBandsRepository.AddAsync"/>
    public async Task AddAsync(Band band)
    {
        await _context.AddAsync(band);
    }
}