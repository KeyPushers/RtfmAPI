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
public class TrackFilesCommandsRepository : BaseCommandsRepository, ITrackFilesCommandsRepository
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
        var transaction = connection.BeginTransaction();

        await InvokeAsync<ITrackFileDomainEvent>(connection, transaction, value, TrackFileDomainEventsHandlerAsync,
            cancellationToken);

        transaction.Commit();
    }

    /// <summary>
    /// Обработчик событий файла музыкального трека.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие.</param>
    private static Task TrackFileDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        ITrackFileDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            TrackFileCreatedDomainEvent dEvent => OnTrackFileCreatedDomainEventAsync(dEvent, connection, transaction),
            TrackFileDataChangedDomainEvent dEvent => OnTrackFileDataChangedDomainEventAsync(dEvent, connection, transaction),
            TrackFileDeletedDomainEvent dEvent => OnTrackFileDeletedDomainEventAsync(dEvent, connection, transaction),
            TrackFileDurationChangedDomainEvent dEvent => OnTrackFileDurationChangedDomainEventAsync(dEvent, connection, transaction),
            TrackFileExtensionChangedDomainEvent dEvent => OnTrackFileExtensionChangedDomainEventAsync(dEvent, connection, transaction),
            TrackFileMimeTypeChangedDomainEvent dEvent => OnTrackFileMimeTypeChangedDomainEventAsync(dEvent, connection, transaction),
            TrackFileNameChangedDomainEvent dEvent => OnTrackFileNameChangedDomainEventAsync(dEvent, connection, transaction),
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };
    }

    /// <summary>
    /// Обработка события изменения названия файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileCreatedDomainEventAsync(TrackFileCreatedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"INSERT INTO trackfiles (id) VALUES (@Id)";
        return connection.ExecuteAsync(sql, new {Id = domainEvent.TrackFileId.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения названия файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileNameChangedDomainEventAsync(TrackFileNameChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE trackfiles SET name = @Name WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Name = domainEvent.TrackFileName.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения содержимого файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileDataChangedDomainEventAsync(TrackFileDataChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE trackfiles SET data = @Data WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Data = domainEvent.TrackFileData.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения продолжительности файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileDurationChangedDomainEventAsync(TrackFileDurationChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE trackfiles SET duration = @Duration WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Duration = domainEvent.TrackFileDuration.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения расширения файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileExtensionChangedDomainEventAsync(TrackFileExtensionChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE trackfiles SET extension = @Extension WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, Extension = domainEvent.TrackFileExtension.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения MIME-типа файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileMimeTypeChangedDomainEventAsync(TrackFileMimeTypeChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE trackfiles SET mime_type = @MimeType WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackFileId.Value, MimeType = domainEvent.TrackFileMimeType.Value}, transaction);
    }

    /// <summary>
    /// Обработка события удаления файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTrackFileDeletedDomainEventAsync(TrackFileDeletedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }
}