using System;
using System.Data;
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
    public async Task CommitChangesAsync(Album value)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            if (domainEvent is AlbumCreatedDomainEvent albumCreatedDomainEvent)
            {
                await OnAlbumCreatedDomainEventAsync(albumCreatedDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is AlbumNameChangedDomainEvent albumNameChangedDomainEvent)
            {
                await OnAlbumNameChangedDomainEventAsync(albumNameChangedDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is AlbumReleaseDateChangedDomainEvent albumReleaseDateChangedDomainEvent)
            {
                await OnAlbumReleaseDateChangedDomainEventAsync(albumReleaseDateChangedDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is AlbumDeletedDomainEvent albumDeletedDomainEvent)
            {
                throw new NotImplementedException();
                continue;
            }
        }

        trx.Commit();
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
    /// обработка события изменения даты выпуска музыкального альбома.
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
}