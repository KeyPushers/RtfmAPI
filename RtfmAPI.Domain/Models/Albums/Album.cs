using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Models.Albums.Events;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums;

/// <summary>
/// Музыкальный альбом.
/// </summary>
public sealed class Album : AggregateRoot<AlbumId, Guid>
{
    private readonly HashSet<TrackId> _trackIds;

    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public AlbumName Name { get; private set; }

    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    public AlbumReleaseDate ReleaseDate { get; private set; }

    /// <summary>
    /// Музыкальные треки.
    /// </summary>
    public IReadOnlyCollection<TrackId> TrackIds => _trackIds;

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="trackIds">Идентификаторы музыкальных треков.</param>
    private Album(AlbumName name, AlbumReleaseDate releaseDate, IEnumerable<TrackId> trackIds) : this(AlbumId.Create(), name, releaseDate, trackIds)
    {
    }

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    /// <param name="id">Идентификатор музыкального альбома.</param>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="trackIds">Идентификаторы музыкальных треков.</param>
    private Album(AlbumId id, AlbumName name, AlbumReleaseDate releaseDate, IEnumerable<TrackId> trackIds) : base(id)
    {
        Name = name;
        ReleaseDate = releaseDate;
        _trackIds = trackIds.ToHashSet();
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="trackIds">Идентификаторы музыкальных треков.</param>
    /// <returns>Музыкальный альбом.</returns>
    internal static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate, IEnumerable<TrackId> trackIds)
    {
        var album = new Album(name, releaseDate, trackIds);
        album.AddDomainEvent(new AlbumCreatedDomainEvent(album));
        album.AddDomainEvent(new AlbumNameChangedDomainEvent(album, name));
        album.AddDomainEvent(new AlbumReleaseDateChangedDomainEvent(album, releaseDate));
        album.AddDomainEvent(new TracksAddedToAlbumDomainEvent(album.Id, album.TrackIds));
        
        return album;
    }

    /// <summary>
    /// Восстановление музыкального альбома.
    /// </summary>
    /// <param name="id">Идентификатор музыкального альбома.</param>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <param name="trackIds">Идентификаторы музыкальных треков.</param>
    /// <returns>Музыкальный альбом.</returns>
    internal static Result<Album> Restore(AlbumId id, AlbumName name, AlbumReleaseDate releaseDate, IEnumerable<TrackId> trackIds)
    {
        return new Album(id, name, releaseDate, trackIds);
    }

    /// <summary>
    /// Изменение названия музыкального альбома.
    /// </summary>
    /// <param name="name">Название альбома.</param>
    public BaseResult SetName(AlbumName name)
    {
        if (Name == name)
        {
            return BaseResult.Success();
        }

        Name = name;
        AddDomainEvent(new AlbumNameChangedDomainEvent(this, name));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    public BaseResult SetReleaseDate(AlbumReleaseDate releaseDate)
    {
        if (ReleaseDate == releaseDate)
        {
            return BaseResult.Success();
        }

        ReleaseDate = releaseDate;
        AddDomainEvent(new AlbumReleaseDateChangedDomainEvent(this, releaseDate));
        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных треков.
    /// </summary>
    /// <param name="trackIds">Идентификаторы добавляемых музыкальных треков.</param>
    public BaseResult AddTracks(IReadOnlyCollection<TrackId> trackIds)
    {
        if (!trackIds.Any())
        {
            return BaseResult.Success();
        }

        List<TrackId> addedTrackIds = new();
        foreach (var trackId in trackIds)
        {
            if (_trackIds.Contains(trackId))
            {
                continue;
            }

            addedTrackIds.Add(trackId);
            _trackIds.Add(trackId);
        }

        AddDomainEvent(new TracksAddedToAlbumDomainEvent(Id, addedTrackIds));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных треков.
    /// </summary>
    /// <param name="trackIds">Идентификаторы удаляемых музыкальных треков.</param>
    public BaseResult RemoveTracks(IReadOnlyCollection<TrackId> trackIds)
    {
        if (!trackIds.Any())
        {
            return BaseResult.Success();
        }

        List<TrackId> removedTrackIds = new();
        foreach (var trackId in trackIds)
        {
            if (!_trackIds.Contains(trackId))
            {
                continue;
            }

            removedTrackIds.Add(trackId);
            _trackIds.Remove(trackId);
        }

        AddDomainEvent(new TracksRemovedFromAlbumDomainEvent(Id, removedTrackIds));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкального альбома.
    /// </summary>
    public BaseResult Delete()
    {
        AddDomainEvent(new AlbumDeletedDomainEvent(this));
        return BaseResult.Success();
    }
}