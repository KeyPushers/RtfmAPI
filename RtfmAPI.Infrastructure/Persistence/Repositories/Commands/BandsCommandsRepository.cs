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
    public async Task CommitChangesAsync(Band value, CancellationToken cancellationToken)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            switch (domainEvent)
            {
                case BandCreatedDomainEvent bandCreatedDomainEvent:
                {
                    await CreateBandAsync(bandCreatedDomainEvent, connection, trx);
                    break;
                }
                case BandNameChangedDomainEvent bandNameChangedDomainEvent:
                {
                    await ChangeBandNameAsync(bandNameChangedDomainEvent, connection, trx);
                    break;
                }
                case AlbumsAddedToBandDomainEvent albumsAddedToBandDomainEvent:
                {
                    await AddAlbumsToBandAsync(albumsAddedToBandDomainEvent, connection, trx);
                    break;
                }
                case AlbumsRemovedFromBandDomainEvent albumsRemovedFromBandDomainEvent:
                {
                    await RemoveAlbumsFromBandAsync(albumsRemovedFromBandDomainEvent, connection, trx);
                    break;
                }
                case GenresAddedToBandDomainEvent genresAddedToBandDomainEvent:
                {
                    await AddGenresToBandAsync(genresAddedToBandDomainEvent, connection, trx);
                    break;
                }
                case GenresRemovedFromBandDomainEvent genresRemovedFromBandDomainEvent:
                {
                    await RemoveGenresFromBandAsync(genresRemovedFromBandDomainEvent, connection, trx);
                    break;
                }
                case BandDeletedDomainEvent bandDeletedDomainEvent:
                {
                    throw new NotImplementedException();
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
    /// Добавление музыкальной группы.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task CreateBandAsync(BandCreatedDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var band = domainEvent.Band;

        const string sql = @"INSERT INTO Bands (Id) VALUES(@Id)";
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

        const string sql = @"UPDATE Bands SET Name = @Name WHERE Id = @Id";
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
            const string sql = @"INSERT INTO BandAlbums (BandId, AlbumId) VALUES(@BandId, @AlbumId)";
            await connection.ExecuteAsync(sql, new {BandId = bandId.Value, AlbumId = albumId.Value}, trx);
        }
    }

    /// <summary>
    /// Удаление музыкальных альбомов из музыкальной группы.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private Task RemoveAlbumsFromBandAsync(AlbumsRemovedFromBandDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var bandId = domainEvent.Band.Id;
        var albumIds = domainEvent.RemovedAlbumIds.Select(entity => entity.Value).ToArray();

        const string sql = @"DELETE FROM BandAlbums WHERE BandId = @BandId AND AlbumId = ANY(@AlbumIds);";
        return connection.ExecuteAsync(sql, new {BandId = bandId.Value, AlbumIds = albumIds}, trx);
    }

    /// <summary>
    /// Добавление музыкальных жанров в музыкальную группу.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private async Task AddGenresToBandAsync(GenresAddedToBandDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var bandId = domainEvent.Band.Id;
        var genreIds = domainEvent.AddedGenreIds;

        foreach (var genreId in genreIds)
        {
            const string sql = @"INSERT INTO BandGenres (BandId, GenreId) VALUES(@BandId, @GenreId)";
            await connection.ExecuteAsync(sql, new {BandId = bandId.Value, GenreId = genreId.Value}, trx);
        }
    }

    /// <summary>
    /// Удаление музыкальных жанров из музыкальной группы.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private Task RemoveGenresFromBandAsync(GenresRemovedFromBandDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        var bandId = domainEvent.Band.Id;
        var genreIds = domainEvent.RemovedGenreIds.Select(entity => entity.Value).ToList();

        const string sql = @"DELETE FROM BandGenres WHERE BandId = @BandId AND GenreId = ANY(@GenreIds)";
        return connection.ExecuteAsync(sql, new {BandId = bandId.Value, GenreIds = genreIds}, trx);
    }
}