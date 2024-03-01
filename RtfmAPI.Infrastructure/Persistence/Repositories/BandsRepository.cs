using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Dao.Dao.Bands;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="Band"/>.
/// </summary>
public class BandsRepository : IBandsRepository
{
    private readonly IMapper _mapper;
    private readonly DbSet<BandDao> _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="Band"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="mapper">Маппер.</param>
    public BandsRepository(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context.Set<BandDao>();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandsAsync"/>
    public Task<List<Band>> GetBandsAsync()
    {
        return _context.Select(entity => _mapper.Map<Band>(entity)).ToListAsync();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandsByAlbumIdAsync"/>
    public Task<List<Band>> GetBandsByAlbumIdAsync(AlbumId albumId)
    {
        return _context
            .Select(entity => _mapper.Map<Band>(entity))
            .ToListAsync();
    }

    /// <inheritdoc cref="IBandsRepository.GetBandByIdAsync"/>
    public async Task<Band?> GetBandByIdAsync(BandId bandId)
    {
        var bandDao = await _context.FirstOrDefaultAsync(entity => entity.Id == bandId.Value);
        return bandDao is null ? null : _mapper.Map<Band>(bandDao);
    }

    /// <inheritdoc cref="IBandsRepository.AddAsync"/>
    public async Task AddAsync(Band band)
    {
        var bandDao = _mapper.Map<BandDao>(band);
        await _context.AddAsync(bandDao);
    }

    /// <inheritdoc cref="IBandsRepository.UpdateAsync"/>
    public Task UpdateAsync(Band band)
    {
        var bandDao = _mapper.Map<BandDao>(band);
        _context.Update(bandDao);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IBandsRepository.DeleteAsync"/>
    public Task<bool> DeleteAsync(Band band)
    {
        var bandDao = _mapper.Map<BandDao>(band);
        var deleteAction = _context.Remove(bandDao);
        return Task.FromResult(deleteAction.State is EntityState.Deleted);
    }
}