using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RftmAPI.Domain.Models.Tracks.Events;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks;

/// <summary>
/// Музыкальный трек.
/// </summary>
public sealed class Track : AggregateRoot<TrackId, Guid>
{
    private HashSet<GenreId> _genreIds = new();

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
    public TrackFileId TrackFileId { get; private set; }

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    public AlbumId? AlbumId { get; private set; }

    /// <summary>
    /// Продолжительность.
    /// </summary>
    public TrackDuration Duration { get; private set; }

    /// <summary>
    /// Музыкальные жанры.
    /// </summary>
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
#pragma warning disable CS8618
    private Track()
    {
    }
#pragma warning restore CS8618

    #region Создание музыкального трека.

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="duration">Продолжительность музыкального трека.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    private Track(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile, Album? album,
        TrackDuration duration,
        IEnumerable<Genre> genres) : base(TrackId.Create())
    {
        Name = name;
        ReleaseDate = releaseDate;
        TrackFileId = TrackFileId.Create(trackFile.Id.Value);
        AlbumId = album?.Id is not null ? AlbumId.Create(album.Id.Value) : null;
        Duration = duration;
        _genreIds = genres.Select(genre => (GenreId) genre.Id).ToHashSet();

        AddDomainEvent(new TrackCreatedDomainEvent(this));
        if (album is not null)
        {
            AddDomainEvent(new AlbumAddedToTrackDomainEvent(this, album));
        }
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <returns>Музыкальный трек.</returns>
    public static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile)
    {
        var trackDurationResult = TrackDuration.Create(trackFile.Duration.Value);
        if (trackDurationResult.IsFailed)
        {
            return trackDurationResult.Error;
        }

        return new Track(name, releaseDate, trackFile, null, trackDurationResult.Value, Enumerable.Empty<Genre>());
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="album">Музыкальный альбом.</param>
    /// <returns>Музыкальный трек.</returns>
    public static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile,
        Album album)
    {
        var trackDurationResult = TrackDuration.Create(trackFile.Duration.Value);
        if (trackDurationResult.IsFailed)
        {
            return trackDurationResult.Error;
        }

        return new Track(name, releaseDate, trackFile, album, trackDurationResult.Value, Enumerable.Empty<Genre>());
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    /// <returns>Музыкальный трек.</returns>
    public static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile,
        IEnumerable<Genre> genres)
    {
        var trackDurationResult = TrackDuration.Create(trackFile.Duration.Value);
        if (trackDurationResult.IsFailed)
        {
            return trackDurationResult.Error;
        }

        return new Track(name, releaseDate, trackFile, null, trackDurationResult.Value, genres);
    }

    /// <summary>
    /// Создание музыкального трека.
    /// </summary>
    /// <param name="name">Название музыкального трека.</param>
    /// <param name="releaseDate">Дата выпуска музыкального трека.</param>
    /// <param name="trackFile">Содержимое файла музыкального трека.</param>
    /// <param name="album">Музыкальный альбом.</param>
    /// <param name="genres">Музыкальные жанры.</param>
    /// <returns>Музыкальный трек.</returns>
    public static Result<Track> Create(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile,
        Album album, IEnumerable<Genre> genres)
    {
        var trackDurationResult = TrackDuration.Create(trackFile.Duration.Value);
        if (trackDurationResult.IsFailed)
        {
            return trackDurationResult.Error;
        }

        return new Track(name, releaseDate, trackFile, album, trackDurationResult.Value, genres);
    }

    #endregion

    /// <summary>
    /// Добавление музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный трек.</param>
    /// <returns>Результат операции.</returns>
    public BaseResult AddAlbum(Album album)
    {
        if (AlbumId?.Value == album.Id.Value)
        {
            return BaseResult.Success();
        }

        AlbumId = AlbumId.Create(album.Id.Value);

        AddDomainEvent(new AlbumAddedToTrackDomainEvent(this, album));

        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкального альбома.
    /// </summary>
    /// <returns>Результат операции.</returns>
    public BaseResult RemoveAlbum()
    {
        if (AlbumId is null)
        {
            return BaseResult.Success();
        }

        var albumId = AlbumId;
        AlbumId = null;
        AddDomainEvent(new AlbumRemovedFromTrackDomainEvent(this, albumId));
        return BaseResult.Success();
    }
}