﻿using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Domain.Models.Tracks;
using RtfmAPI.Domain.Models.Tracks.Events;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Репозиторий команд доменной модели <see cref="Track"/>.
/// </summary>
public class TracksCommandsRepository : ITracksCommandsRepository
{
    private readonly DataContext _context;

    /// <summary>
    /// Создание репозитория комманд доменной модели <see cref="Track"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public TracksCommandsRepository(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task CommitChangesAsync(Track value, CancellationToken cancellationToken = default)
    {
        var connection = _context.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            switch (domainEvent)
            {
                case TrackCreatedDomainEvent trackCreatedDomainEvent:
                {
                    await OnTrackCreatedDomainEventAsync(trackCreatedDomainEvent, connection, trx);
                    break;
                }
                case TrackNameChangedDomainEvent trackNameChangedDomainEvent:
                {
                    await OnTrackNameChangedDomainEventAsync(trackNameChangedDomainEvent, connection, trx);
                    break;
                }
                case TrackReleaseDateChangedDomainEvent trackReleaseDateChangedDomainEvent:
                {
                    await OnTrackReleaseDateChangedDomainEventAsync(trackReleaseDateChangedDomainEvent, connection,
                        trx);
                    break;
                }
                case GenresAddedToTrackDomainEvent genresAddedToTrackDomainEvent:
                {
                    await OnGenresAddedToTrackDomainEventAsync(genresAddedToTrackDomainEvent, connection, trx);
                    break;
                }
                case GenresRemovedFromTrackDomainEvent genresRemovedFromTrackDomainEvent:
                {
                    await OnGenresRemovedFromTrackDomainEventAsync(genresRemovedFromTrackDomainEvent, connection, trx);
                    break;
                }
                case TrackFileChangedInTrackDomainEvent trackFileChangedInTrackDomainEvent:
                {
                    await OnTrackFileChangedInTrackDomainEventAsync(trackFileChangedInTrackDomainEvent, connection,
                        trx);
                    break;
                }
                case TrackDeletedDomainEvent trackDeletedDomainEvent:
                {
                    throw new NotImplementedException();
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
    /// Обработка события создания музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task OnTrackCreatedDomainEventAsync(TrackCreatedDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction trx)
    {
        const string sql = @"INSERT INTO tracks(id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = domainEvent.TrackId.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения названия музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task OnTrackNameChangedDomainEventAsync(TrackNameChangedDomainEvent domainEvent,
        IDbConnection connection,
        IDbTransaction trx)
    {
        const string sql = @"UPDATE tracks SET name = @Name WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = domainEvent.TrackId.Value, Name = domainEvent.Name.Value}, trx);
    }

    /// <summary>
    /// Обработка события изменения даты выпуска музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task OnTrackReleaseDateChangedDomainEventAsync(TrackReleaseDateChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE tracks SET release_date = @ReleaseDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackId.Value, ReleaseDate = domainEvent.ReleaseDate.Value}, trx);
    }

    /// <summary>
    /// Обработка события добавления жанров к музыкальному треку.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private static async Task OnGenresAddedToTrackDomainEventAsync(GenresAddedToTrackDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        var trackId = domainEvent.TrackId;
        var genreIds = domainEvent.AddedGenreIds;

        foreach (var genreId in genreIds)
        {
            const string sql = @"INSERT INTO tracks_genres(track_id, genre_id) VALUES(@TrackId, @GenreId)";
            await connection.ExecuteAsync(sql, new {TrackId = trackId.Value, GenreId = genreId.Value}, trx);
        }
    }

    /// <summary>
    /// Обработка события удаления жанров из музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task OnGenresRemovedFromTrackDomainEventAsync(GenresRemovedFromTrackDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        var trackId = domainEvent.TrackId;
        var genreIds = domainEvent.RemovedGenreIds.Select(entity => entity.Value);

        const string sql = @"DELETE FROM tracks_genres WHERE track_id = @TrackId AND genre_id = ANY(@GenreIds)";
        return connection.ExecuteAsync(sql, new {TrackId = trackId.Value, GenreIds = genreIds}, trx);
    }

    /// <summary>
    /// Обработка события изменения файла музыкального трека.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task OnTrackFileChangedInTrackDomainEventAsync(TrackFileChangedInTrackDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"UPDATE tracks SET trackfile_id = @TrackFileId WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = domainEvent.TrackId.Value, TrackFileId = domainEvent.TrackFileId.Value}, trx);
    }
}