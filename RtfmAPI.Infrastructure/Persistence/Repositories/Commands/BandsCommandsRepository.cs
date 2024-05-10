using System;
using System.Data;
using System.Linq;
using System.Threading;
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
public class BandsCommandsRepository : BaseCommandsRepository, IBandsCommandsRepository
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
    public async Task CommitChangesAsync(Band value, CancellationToken cancellationToken)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var transaction = connection.BeginTransaction();

        await InvokeAsync<IBandDomainEvent>(connection, transaction, value, BandDomainEventsHandlerAsync,
            cancellationToken);

        transaction.Commit();
    }

    /// <summary>
    /// Обработчик событий музыкальной группы.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие.</param>
    private static Task BandDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IBandDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            AlbumsAddedToBandDomainEvent dEvent => OnAlbumsAddedToBandDomainEventAsync(dEvent, connection, transaction),
            AlbumsRemovedFromBandDomainEvent dEvent => OnAlbumsRemovedFromBandDomainEventAsync(dEvent, connection, transaction),
            BandCreatedDomainEvent dEvent => OnBandCreatedDomainEventAsync(dEvent, connection, transaction),
            BandDeletedDomainEvent dEvent => OnBandDeletedDomainEventAsync(dEvent, connection, transaction),
            BandNameChangedDomainEvent dEvent => OnBandNameChangedDomainEventAsync(dEvent, connection, transaction),
            GenresAddedToBandDomainEvent dEvent => OnGenresAddedToBandDomainEventAsync(dEvent, connection, transaction),
            GenresRemovedFromBandDomainEvent dEvent => OnGenresRemovedFromBandDomainEventAsync(dEvent, connection, transaction),
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };
    }

    /// <summary>
    /// Добавление музыкальной группы.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnBandCreatedDomainEventAsync(BandCreatedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var band = dEvent.Band;

        const string sql = @"INSERT INTO bands (id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = band.Id.Value, Name = string.Empty}, transaction);
    }

    /// <summary>
    /// Изменение названия музыкальной группы.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnBandNameChangedDomainEventAsync(BandNameChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var band = dEvent.Band;

        const string sql = @"UPDATE bands SET name = @Name WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = band.Id.Value, Name = band.Name.Value}, transaction);
    }

    /// <summary>
    /// Добавление музыкальных альбомов в музыкальную группу.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static async Task OnAlbumsAddedToBandDomainEventAsync(AlbumsAddedToBandDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var bandId = dEvent.Band.Id;
        var albumIds = dEvent.AddedAlbumIds;

        foreach (var albumId in albumIds)
        {
            const string sql = @"INSERT INTO bands_albums (band_id, album_id) VALUES(@BandId, @AlbumId)";
            await connection.ExecuteAsync(sql, new {BandId = bandId.Value, AlbumId = albumId.Value}, transaction);
        }
    }

    /// <summary>
    /// Обработка события удаления музыкальной группы
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnBandDeletedDomainEventAsync(BandDeletedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удаление музыкальных альбомов из музыкальной группы.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnAlbumsRemovedFromBandDomainEventAsync(AlbumsRemovedFromBandDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var bandId = dEvent.Band.Id;
        var albumIds = dEvent.RemovedAlbumIds.Select(entity => entity.Value).ToArray();

        const string sql = @"DELETE FROM bands_albums WHERE band_id = @BandId AND album_id = ANY(@AlbumIds);";
        return connection.ExecuteAsync(sql, new {BandId = bandId.Value, AlbumIds = albumIds}, transaction);
    }

    /// <summary>
    /// Добавление музыкальных жанров в музыкальную группу.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static async Task OnGenresAddedToBandDomainEventAsync(GenresAddedToBandDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var bandId = dEvent.Band.Id;
        var genreIds = dEvent.AddedGenreIds;

        foreach (var genreId in genreIds)
        {
            const string sql = @"INSERT INTO bands_genres (band_id, genre_id) VALUES(@BandId, @GenreId)";
            await connection.ExecuteAsync(sql, new {BandId = bandId.Value, GenreId = genreId.Value}, transaction);
        }
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкальной группы.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnGenresRemovedFromBandDomainEventAsync(GenresRemovedFromBandDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var bandId = dEvent.Band.Id;
        var genreIds = dEvent.RemovedGenreIds.Select(entity => entity.Value).ToList();

        const string sql = @"DELETE FROM bands_genres WHERE band_id = @BandId AND genre_id = ANY(@GenreIds)";
        return connection.ExecuteAsync(sql, new {BandId = bandId.Value, GenreIds = genreIds}, transaction);
    }
}