using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий доменной модели <see cref="TrackFile"/>.
/// </summary>
public class TrackFilesRepository : ITrackFilesRepository
{
    private readonly IMapper _mapper;
    private readonly DbSet<TrackFileDao> _context;

    /// <summary>
    /// Репозиторий доменной модели <see cref="TrackFile"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="mapper">Маппер.</param>
    public TrackFilesRepository(AppDbContext context, IMapper mapper)
    {
        _context = context.Set<TrackFileDao>();
        _mapper = mapper;
    }

    /// <see cref="ITrackFilesRepository.GetTrackFileByIdAsync"/>
    public async Task<TrackFile?> GetTrackFileByIdAsync(TrackFileId trackFileId)
    {
        var value = await _context.FirstOrDefaultAsync(trackFile => trackFile.Id == trackFileId.Value);
        return value is null ? null : _mapper.Map<TrackFile>(value);
    }

    /// <see cref="ITrackFilesRepository.AddAsync"/>
    public async Task AddAsync(TrackFile trackFile)
    {
        var trackFileDao = _mapper.Map<TrackFileDao>(trackFile);
        await _context.AddAsync(trackFileDao);
    }

    /// <inheritdoc cref="ITrackFilesRepository.DeleteAsync"/>
    public Task<bool> DeleteAsync(TrackFile trackFile)
    {
        var trackFileDao = _mapper.Map<TrackFileDao>(trackFile);
        var removeResult = _context.Remove(trackFileDao);
        return Task.FromResult(removeResult.State is EntityState.Deleted);
    }
}