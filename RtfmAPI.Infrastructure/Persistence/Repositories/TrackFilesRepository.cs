using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="TrackFile"/>.
/// </summary>
public class TrackFilesRepository : ITrackFilesRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="TrackFile"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TrackFilesRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <see cref="ITrackFilesRepository.GetTrackFileByIdAsync"/>
    public Task<TrackFile?> GetTrackFileByIdAsync(TrackFileId trackFileId)
    {
        return _context.Set<TrackFile>().FirstOrDefaultAsync(trackFile => trackFile.Id == trackFileId);
    }

    /// <see cref="ITrackFilesRepository.AddAsync"/>
    public async Task AddAsync(TrackFile trackFile)
    {
        await _context.AddAsync(trackFile);
    }

    /// <inheritdoc cref="ITrackFilesRepository.DeleteAsync"/>
    public Task<bool> DeleteAsync(TrackFile trackFile)
    {
        var removeResult = _context.Remove(trackFile);
        return Task.FromResult(removeResult.State is EntityState.Deleted);
    }
}