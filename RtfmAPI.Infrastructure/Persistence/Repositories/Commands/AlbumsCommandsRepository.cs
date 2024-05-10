using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.Events;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Репозиторий команд доменной модели <see cref="Album"/>.
/// </summary>
public class AlbumsCommandsRepository : BaseCommandsRepository, IAlbumsCommandsRepository
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Создание репозитория команд доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    public AlbumsCommandsRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc/>
    public async Task CommitChangesAsync(Album value, CancellationToken cancellationToken)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var transaction = connection.BeginTransaction();

        await InvokeAsync<IAlbumDomainEvent>(connection, transaction, value, AlbumDomainEventsHandlerAsync,
            cancellationToken);

        transaction.Commit();
    }

    /// <summary>
    /// Обработчик событий музыкального альбома.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие.</param>
    /// <exception cref="ArgumentOutOfRangeException">Неизвестное доменное событие.</exception>
    private static Task AlbumDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IAlbumDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            AlbumCreatedDomainEvent dEvent => OnAlbumCreatedDomainEventAsync(dEvent, connection, transaction),
            AlbumDeletedDomainEvent dEvent => OnAlbumDeleteDomainEventAsync(dEvent, connection, transaction),
            AlbumNameChangedDomainEvent dEvent => OnAlbumNameChangedDomainEventAsync(dEvent, connection, transaction),
            AlbumReleaseDateChangedDomainEvent dEvent => OnAlbumReleaseDateChangedDomainEventAsync(dEvent, connection, transaction),
            TracksAddedToAlbumDomainEvent dEvent => OnTracksAddedToAlbumDomainEventAsync(dEvent, connection, transaction),
            TracksRemovedFromAlbumDomainEvent dEvent => OnTracksRemovedFromAlbumDomainEventAsync(dEvent, connection, transaction),
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };
    }

    /// <summary>
    /// Обработка события создания музыкального альбома.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnAlbumCreatedDomainEventAsync(AlbumCreatedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var album = dEvent.Album;

        const string sql = @"INSERT INTO albums (id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = album.Id.Value}, transaction);
    }

    /// <summary>
    /// Обработчик события удаления музыкального альбома.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnAlbumDeleteDomainEventAsync(AlbumDeletedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Обработка события изменения названия музыкального альбома.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnAlbumNameChangedDomainEventAsync(AlbumNameChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var album = dEvent.Album;

        const string sql = @"UPDATE albums SET name = @Name WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = album.Id.Value, Name = album.Name.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnAlbumReleaseDateChangedDomainEventAsync(AlbumReleaseDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var album = dEvent.Album;

        const string sql = @"UPDATE albums SET released_date = @ReleaseDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = album.Id.Value, ReleaseDate = album.ReleaseDate.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события добавления музыкальных треков.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static async Task OnTracksAddedToAlbumDomainEventAsync(TracksAddedToAlbumDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var albumId = domainEvent.AlbumId;
        var trackIds = domainEvent.AddedTrackIds;

        foreach (var trackId in trackIds)
        {
            const string sql = @"INSERT INTO albums_tracks (album_id, track_id) VALUES(@AlbumId, @TrackId)";
            await connection.ExecuteAsync(sql, new {AlbumId = albumId.Value, TrackId = trackId.Value}, transaction);
        }
    }

    /// <summary>
    /// Обработка события удаления музыкальных треков.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnTracksRemovedFromAlbumDomainEventAsync(TracksRemovedFromAlbumDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var albumId = dEvent.AlbumId;
        var trackIds = dEvent.RemovedTrackIds.Select(entity => entity.Value).ToList();

        const string sql = @"DELETE FROM albums_tracks WHERE album_id = @AlbumId AND track_id = ANY(@TrackIds)";
        return connection.ExecuteAsync(sql, new {AlbumId = albumId.Value, TrackIds = trackIds}, transaction);
    }
}