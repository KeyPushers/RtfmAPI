﻿using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Models.Tracks.Events;
using RtfmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Tracks;

/// <summary>
/// Музыкальный трек.
/// </summary>
public sealed class Track : AggregateRoot<TrackId, Guid>
{
    private HashSet<GenreId> _genreIds;

    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    public TrackName Name { get; private set; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public TrackReleaseDate ReleaseDate { get; private set; }

    /// <summary>
    /// Файл музыкального трека.
    /// </summary>
    public TrackFileId? TrackFileId { get; private set; }

    /// <summary>
    /// Музыкальные жанры.
    /// </summary>
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    private Track(TrackName name, TrackReleaseDate releaseDate, TrackFileId? trackFileId, IEnumerable<GenreId> genreIds)
        : this(TrackId.Create(), name, releaseDate, trackFileId, genreIds)
    {
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="id">Идентификатор музыкального трека.</param>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    private Track(TrackId id, TrackName name, TrackReleaseDate releaseDate, TrackFileId? trackFileId,
        IEnumerable<GenreId> genreIds) : base(id)
    {
        Name = name;
        ReleaseDate = releaseDate;
        TrackFileId = trackFileId;
        _genreIds = genreIds.ToHashSet();
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    /// <returns>Музыкальный трек.</returns>
    internal static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFileId? trackFileId,
        IEnumerable<GenreId> genreIds)
    {
        var track = new Track(name, releaseDate, trackFileId, genreIds);

        track.AddDomainEvent(new TrackCreatedDomainEvent(track.Id));
        track.AddDomainEvent(new TrackNameChangedDomainEvent(track.Id, track.Name));
        track.AddDomainEvent(new TrackReleaseDateChangedDomainEvent(track.Id, track.ReleaseDate));
        if (track.TrackFileId is not null)
        {
            track.AddDomainEvent(new TrackFileChangedInTrackDomainEvent(track.Id, track.TrackFileId));
        }

        track.AddDomainEvent(new GenresAddedToTrackDomainEvent(track.Id, track.GenreIds));

        return track;
    }

    /// <summary>
    /// Восстановление музыкального трека.
    /// </summary>
    /// <param name="id">Идентификатор музыкального трека.</param>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFileId">Идентификатор файла музыкального трека.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    /// <returns>Музыкальный трек.</returns>
    internal static Result<Track> Restore(TrackId id, TrackName name, TrackReleaseDate releaseDate,
        TrackFileId? trackFileId,
        IEnumerable<GenreId> genreIds)
    {
        return new Track(id, name, releaseDate, trackFileId, genreIds);
        ;
    }

    /// <summary>
    /// Установление названия музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    public BaseResult SetName(TrackName name)
    {
        if (Name == name)
        {
            return BaseResult.Success();
        }

        Name = name;
        AddDomainEvent(new TrackNameChangedDomainEvent(Id, name));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение даты выпуска музыкального трека.
    /// </summary>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    public BaseResult SetReleaseDate(TrackReleaseDate releaseDate)
    {
        if (ReleaseDate == releaseDate)
        {
            return BaseResult.Success();
        }

        ReleaseDate = releaseDate;
        AddDomainEvent(new TrackReleaseDateChangedDomainEvent(Id, releaseDate));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение файла музыкального трека.
    /// </summary>
    /// <param name="file">Файл музыкального трека.</param>
    public BaseResult SetTrackFile(TrackFile file)
    {
        if (TrackFileId is not null && file.Id == TrackFileId)
        {
            return BaseResult.Success();
        }

        TrackFileId = file.Id;

        AddDomainEvent(new TrackFileChangedInTrackDomainEvent(Id, TrackFileId));
        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных жанров.
    /// </summary>
    /// <param name="genreIds">Идентификаторы добавляемых музыкальных жанров.</param>
    public BaseResult AddGenres(IReadOnlyCollection<GenreId> genreIds)
    {
        if (!genreIds.Any())
        {
            return BaseResult.Success();
        }

        List<GenreId> addedGenreIds = new();
        foreach (var genreId in genreIds)
        {
            if (_genreIds.Contains(genreId))
            {
                continue;
            }

            addedGenreIds.Add(genreId);
            _genreIds.Add(genreId);
        }

        AddDomainEvent(new GenresAddedToTrackDomainEvent(Id, addedGenreIds));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров.
    /// </summary>
    /// <param name="genreIds">Идентификаторы удаляемых музыкальных жанров.</param>
    public BaseResult RemoveGenres(IReadOnlyCollection<GenreId> genreIds)
    {
        if (!genreIds.Any())
        {
            return BaseResult.Success();
        }

        List<GenreId> removedGenreIds = new();
        foreach (var genreId in genreIds)
        {
            if (!_genreIds.Contains(genreId))
            {
                continue;
            }

            removedGenreIds.Add(genreId);
            _genreIds.Remove(genreId);
        }

        AddDomainEvent(new GenresRemovedFromTrackDomainEvent(Id, removedGenreIds));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкального трека.
    /// </summary>
    public BaseResult Delete()
    {
        AddDomainEvent(new TrackDeletedDomainEvent(Id));
        return BaseResult.Success();
    }
}