using System;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;
using RtfmAPI.Infrastructure.Daos;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов доменной модели <see cref="TrackFile"/>.
/// </summary>
public class TrackFilesQueriesRepository : ITrackFilesQueriesRepository
{
    private readonly DataContext _context;

    /// <summary>
    /// Создание репозитория запросов доменной модели <see cref="TrackFile"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TrackFilesQueriesRepository(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Result<TrackFile>> GetTrackFileByIdAsync(TrackFileId trackFileId)
    {
        var connection = _context.CreateOpenedConnection();
        const string sql = @"SELECT * From TrackFiles WHERE Id = @TrackFileId";
        var trackFileDao =
            await connection.QuerySingleOrDefaultAsync<TrackFileDao>(sql, new {TrackFileId = trackFileId.Value});
        if (trackFileDao is null)
        {
            return new InvalidOperationException();
        }

        var trackFilesFabric = new TrackFilesFabric(trackFileDao.Name ?? string.Empty, trackFileDao.Data,
            trackFileDao.Extension ?? string.Empty, trackFileDao.MimeType ?? string.Empty, trackFileDao.Duration);
        return trackFilesFabric.Restore(trackFileDao.Id);
    }

    /// <inheritdoc />
    public Task<bool> IsTrackFileExistsAsync(TrackFileId trackFileId)
    {
        var connection = _context.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM TrackFiles WHERE Id=@TrackFileId)";

        return connection.ExecuteScalarAsync<bool>(sql, new {TrackFileId = trackFileId.Value});
    }
}