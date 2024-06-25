using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.Events;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Репозиторий команд доменной модели <see cref="TrackFile"/>.
/// </summary>
public class TrackFilesCommandsRepository : ITrackFilesCommandsRepository
{
    private readonly DataContext _context;

    /// <summary>
    /// Создание репозитория команд доменной модели <see cref="TrackFile"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TrackFilesCommandsRepository(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task CommitChangesAsync(TrackFile value, CancellationToken cancellationToken = default)
    {
        var connection = _context.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            switch (domainEvent)
            {
                case TrackFileCreatedDomainEvent trackFileCreatedDomainEvent:
                {
                    await OnTrackFileCreatedDomainEventAsync(trackFileCreatedDomainEvent, connection, trx);
                    break;
                }
                case TrackFileNameChangedDomainEvent trackFileNameChangedDomainEvent:
                {
                    await OnTrackFileNameChangedDomainEventAsync(trackFileNameChangedDomainEvent, connection, trx);
                    break;
                }
                case TrackFileDataChangedDomainEvent trackFileDataChangedDomainEvent:
                {
                    await OnTrackFileDataChangedDomainEventAsync(trackFileDataChangedDomainEvent, connection, trx);
                    break;
                }
                case TrackFileDurationChangedDomainEvent trackFileDurationChangedDomainEvent:
                {
                    await OnTrackFileDurationChangedDomainEventAsync(trackFileDurationChangedDomainEvent, connection,
                        trx);
                    break;
                }
                case TrackFileExtensionChangedDomainEvent trackFileExtensionChangedDomainEvent:
                {
                    await OnTrackFileExtensionChangedDomainEventAsync(trackFileExtensionChangedDomainEvent, connection,
                        trx);
                    break;
                }
                case TrackFileMimeTypeChangedDomainEvent trackFileMimeTypeChangedDomainEvent:
                {
                    await OnTrackFileMimeTypeChangedDomainEventAsync(trackFileMimeTypeChangedDomainEvent, connection,
                        trx);
                    break;
                }
                case TrackFileDeletedDomainEvent trackFileDeletedDomainEvent:
                {
                    await OnTrackFileDeletedDomainEventAsync(trackFileDeletedDomainEvent, connection, trx);
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(domainEvent));
                }
            }
        }

        trx.Commit();
        value.ClearDomainEvents();
    }

    /// <summary>
    /// Обработка события изменения названия файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileCreatedDomainEventAsync(TrackFileCreatedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"INSERT INTO TrackFiles (Id) VALUES (@Id)";
        return connection.ExecuteAsync(sql, new {Id = domainEvent.TrackFileId.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения названия файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileNameChangedDomainEventAsync(TrackFileNameChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE TrackFiles SET Name = @Name WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Name = domainEvent.TrackFileName.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения содержимого файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileDataChangedDomainEventAsync(TrackFileDataChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE TrackFiles SET Data = @Data WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Data = domainEvent.TrackFileData.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileDurationChangedDomainEventAsync(TrackFileDurationChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE TrackFiles SET Duration = @Duration WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Duration = domainEvent.TrackFileDuration.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения расширения файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileExtensionChangedDomainEventAsync(TrackFileExtensionChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE TrackFiles SET Extension = @Extension WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Extension = domainEvent.TrackFileExtension.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения MIME-типа файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileMimeTypeChangedDomainEventAsync(TrackFileMimeTypeChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE TrackFiles SET MimeType = @MimeType WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, MimeType = domainEvent.TrackFileMimeType.Value}, trx);
    }

    /// <summary>
    /// Обработка события удаления файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private Task OnTrackFileDeletedDomainEventAsync(TrackFileDeletedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        throw new NotImplementedException();
    }
}