using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.Events;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Репозиторий команд доменной модели <see cref="Band"/>.
/// </summary>
public class BandsCommandsRepository : IBandsCommandsRepository
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Создание репозитория команд доменной модели <see cref="Band"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    public BandsCommandsRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc/>
    public async Task CommitChangesAsync(Band value)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            if (domainEvent is BandCreatedDomainEvent bandCreatedDomainEvent)
            {
                await AddBandAsync(bandCreatedDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is BandNameChangedDomainEvent bandNameChangedDomainEvent)
            {
                await ChangeBandNameAsync(bandNameChangedDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is AlbumsAddedToBandDomainEvent albumsAddedToBandDomainEvent)
            {
                await AddAlbumsToBandAsync(albumsAddedToBandDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is AlbumsRemovedFromBandDomainEvent albumsRemovedFromBandDomainEvent)
            {
                await RemoveAlbumsFromBandAsync(albumsRemovedFromBandDomainEvent, connection, trx);
                continue;
            }

            if (domainEvent is BandDeletedDomainEvent bandDeletedDomainEvent)
            {
                throw new NotImplementedException();
                continue;
            }
        }

        trx.Commit();
    }

    /// <summary>
    /// Добавление музыкальной группы.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task AddBandAsync(BandCreatedDomainEvent domainEvent, IDbConnection connection, IDbTransaction trx)
    {
        var band = domainEvent.Band;

        var sql = """
                    INSERT INTO Bands (Id) VALUES(@Id)
                  """;
        return connection.ExecuteAsync(sql, new {Id = band.Id.Value, Name = string.Empty}, trx);
    }

    /// <summary>
    /// Изменение названия музыкальной группы.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private Task ChangeBandNameAsync(BandNameChangedDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var band = domainEvent.Band;

        var sql = """
                    UPDATE Bands SET Name = @Name WHERE Id = @Id
                  """;
        return connection.ExecuteAsync(sql, new {Id = band.Id.Value, Name = band.Name.Value}, trx);
    }

    /// <summary>
    /// Добавление музыкальных альбомов в музыкальную группу.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private async Task AddAlbumsToBandAsync(AlbumsAddedToBandDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var bandId = domainEvent.Band.Id;
        var albumIds = domainEvent.AddedAlbumIds;

        foreach (var albumId in albumIds)
        {
            var sql = @"INSERT INTO BandAlbums (BandId, AlbumId) VALUES(@BandId, @AlbumId)";
            await connection.ExecuteAsync(sql, new {BandId = bandId.Value, AlbumId = albumId.Value}, trx);
        }
    }

    /// <summary>
    /// Удаление музыкальных альбомов из музыкальной группы.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private async Task RemoveAlbumsFromBandAsync(AlbumsRemovedFromBandDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var bandId = domainEvent.Band.Id;
        var albumIds = domainEvent.RemovedAlbumIds;

        foreach (var albumId in albumIds)
        {
            var sql = @"DELETE FROM BandAlbums WHERE BandId = @BandId AND AlbumId = @AlbumId";
            await connection.ExecuteAsync(sql, new {BandId = bandId.Value, AlbumId = albumId.Value}, trx);
        }
    }
}