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
public class AlbumsCommandsRepository : IAlbumsCommandsRepository
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
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            switch (domainEvent)
            {
                case AlbumCreatedDomainEvent albumCreatedDomainEvent:
                {
                    await OnAlbumCreatedDomainEventAsync(albumCreatedDomainEvent, connection, trx);
                    break;
                }
                case AlbumNameChangedDomainEvent albumNameChangedDomainEvent:
                {
                    await OnAlbumNameChangedDomainEventAsync(albumNameChangedDomainEvent, connection, trx);
                    break;
                }
                case AlbumReleaseDateChangedDomainEvent albumReleaseDateChangedDomainEvent:
                {
                    await OnAlbumReleaseDateChangedDomainEventAsync(albumReleaseDateChangedDomainEvent, connection,
                        trx);
                    break;
                }
                case TracksAddedToAlbumDomainEvent tracksAddedToAlbumDomainEvent:
                {
                    await OnTracksAddedToAlbumDomainEventAsync(tracksAddedToAlbumDomainEvent, connection, trx);
                    break;
                }
                case TracksRemovedFromAlbumDomainEvent tracksRemovedFromAlbumDomainEvent:
                {
                    await OnTracksRemovedFromAlbumDomainEventAsync(tracksRemovedFromAlbumDomainEvent, connection, trx);
                    break;
                }
                case AlbumDeletedDomainEvent albumDeletedDomainEvent:
                {
                    throw new NotImplementedException();
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        trx.Commit();
        value.ClearDomainEvents();
    }

    /// <summary>
    /// Обработка события создания музыкального альбома.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnAlbumCreatedDomainEventAsync(AlbumCreatedDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var album = domainEvent.Album;

        var sql = """
                    INSERT INTO Albums (Id) VALUES(@Id)
                  """;
        return connection.ExecuteAsync(sql, new {Id = album.Id.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения названия музыкального альбома.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnAlbumNameChangedDomainEventAsync(AlbumNameChangedDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var album = domainEvent.Album;

        var sql = """
                    UPDATE Albums SET Name = @Name WHERE Id = @Id
                  """;
        return connection.ExecuteAsync(sql, new {Id = album.Id.Value, Name = album.Name.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnAlbumReleaseDateChangedDomainEventAsync(AlbumReleaseDateChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var album = domainEvent.Album;

        var sql = """
                    UPDATE Albums SET ReleaseDate = @ReleaseDate WHERE Id = @Id
                  """;
        return connection.ExecuteAsync(sql, new {Id = album.Id.Value, ReleaseDate = album.ReleaseDate.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события добавления музыкальных треков.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private async Task OnTracksAddedToAlbumDomainEventAsync(TracksAddedToAlbumDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var albumId = domainEvent.AlbumId;
        var trackIds = domainEvent.AddedTrackIds;

        foreach (var trackId in trackIds)
        {
            var sql = @"INSERT INTO AlbumTracks (AlbumId, TrackId) VALUES(@AlbumId, @TrackId)";
            await connection.ExecuteAsync(sql, new {AlbumId = albumId.Value, TrackId = trackId.Value}, transaction);
        }
    }

    /// <summary>
    /// Обработка события удаления музыкальных треков.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnTracksRemovedFromAlbumDomainEventAsync(TracksRemovedFromAlbumDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var albumId = domainEvent.AlbumId;
        var trackIds = domainEvent.RemovedTrackIds.Select(entity => entity.Value).ToList();

        var sql = @"DELETE FROM AlbumTracks WHERE AlbumId = @AlbumId AND TrackId = ANY(@TrackIds)";
        return connection.ExecuteAsync(sql, new {AlbumId = albumId.Value, TrackIds = trackIds}, transaction);
    }
}