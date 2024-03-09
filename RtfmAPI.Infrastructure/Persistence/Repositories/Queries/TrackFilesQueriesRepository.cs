using System;
using System.Data;
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
        var trackFileDao = await GetTrackFileDaoAsync(trackFileId.Value, connection);
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

    /// <inheritdoc />
    public async Task<double> GetTrackFileDurationAsync(TrackFileId trackFileId)
    {
        var connection = _context.CreateOpenedConnection();
        var trackFileDao = await GetTrackFileDaoAsync(trackFileId.Value, connection);
        return trackFileDao?.Duration ?? 0.0;
    }

    /// <summary>
    /// Получение объекта доступа данных файла музыкального трека.
    /// </summary>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    /// <param name="connection">Соединение.</param>
    /// <returns>Объект доступа данных файла музыкального трека.</returns>
    private static Task<TrackFileDao?> GetTrackFileDaoAsync(Guid trackFileId, IDbConnection connection)
    {
        const string sql = @"SELECT * From TrackFiles WHERE Id = @TrackFileId";
        return connection.QuerySingleOrDefaultAsync<TrackFileDao>(sql, new {TrackFileId = trackFileId});
    }
}