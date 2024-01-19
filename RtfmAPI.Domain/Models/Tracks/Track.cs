using RftmAPI.Domain.Exceptions.TrackExceptions;
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
            AddDomainEvent(new AlbumChangedInTrackDomainEvent(this, album));
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
        var trackResult = CreateTrack(name, releaseDate, trackFile, null, Enumerable.Empty<Genre>());
        return trackResult.IsFailed ? trackResult.Error : trackResult;
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
        var trackResult = CreateTrack(name, releaseDate, trackFile, album, Enumerable.Empty<Genre>());
        return trackResult.IsFailed ? trackResult.Error : trackResult;
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
        var trackResult = CreateTrack(name, releaseDate, trackFile, null, genres);
        return trackResult.IsFailed ? trackResult.Error : trackResult;
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
        var trackResult = CreateTrack(name, releaseDate, trackFile, album, genres);
        return trackResult.IsFailed ? trackResult.Error : trackResult;
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
    private static Result<Track> CreateTrack(TrackName name, TrackReleaseDate releaseDate, TrackFile trackFile,
        Album? album, IEnumerable<Genre> genres)
    {
        var trackDurationResult = TrackDuration.Create(trackFile.Duration.Value);
        return trackDurationResult.IsFailed
            ? trackDurationResult.Error
            : new Track(name, releaseDate, trackFile, album, trackDurationResult.Value, genres);
    }

    #endregion

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
        AddDomainEvent(new TrackNameChangedDomainEvent(this, name));
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
        AddDomainEvent(new TrackReleaseDateChangedDomainEvent(this, releaseDate));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение файла музыкального трека.
    /// </summary>
    /// <param name="file">Файл музыкального трека.</param>
    public BaseResult SetTrackFile(TrackFile file)
    {
        if (file.Id == TrackFileId)
        {
            return BaseResult.Success();
        }

        TrackFileId = TrackFileId.Create(file.Id.Value);

        AddDomainEvent(new TrackFileChangedInTrackDomainEvent(this, file));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение музыкального альбома.
    /// </summary>
    /// <param name="album">Музыкальный альбома.</param>
    public BaseResult SetAlbum(Album album)
    {
        if (AlbumId?.Value == album.Id.Value)
        {
            return BaseResult.Success();
        }

        AlbumId = AlbumId.Create(album.Id.Value);

        AddDomainEvent(new AlbumChangedInTrackDomainEvent(this, album));
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

    /// <summary>
    /// Добавление музыкальных жанров.
    /// </summary>
    /// <param name="genres">Добавляемые жанры.</param>
    public BaseResult AddGenres(IReadOnlyCollection<Genre> genres)
    {
        if (!genres.Any())
        {
            return BaseResult.Success();
        }

        List<Genre> addedGenres = new();
        foreach (var genre in genres)
        {
            var genreId = GenreId.Create(genre.Id.Value);
            if (_genreIds.Contains(genreId))
            {
                continue;
            }

            addedGenres.Add(genre);
            _genreIds.Add(genreId);
        }

        AddDomainEvent(new GenresAddedToTrackDomainEvent(this, addedGenres));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных жанров.
    /// </summary>
    /// <param name="genres">Удаляемые жанры.</param>
    public BaseResult RemoveGenres(IReadOnlyCollection<Genre> genres)
    {
        if (!genres.Any())
        {
            return BaseResult.Success();
        }

        List<Genre> removedGenres = new();
        foreach (var genre in genres)
        {
            var genreId = GenreId.Create(genre.Id.Value);
            if (!_genreIds.Contains(genreId))
            {
                continue;
            }

            removedGenres.Add(genre);
            _genreIds.Remove(genreId);
        }

        AddDomainEvent(new GenresRemovedFromTrackDomainEvent(this, removedGenres));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкального трека.
    /// </summary>
    /// <param name="deleteAction">Делегат, отвечающий за удаление музыкального трека.</param>
    public async Task<BaseResult> DeleteAsync(Func<Track, Task<bool>> deleteAction)
    {
        var trackId = (TrackId) Id;
        var deleteActionResult = await deleteAction(this);
        if (!deleteActionResult)
        {
            return TrackExceptions.DeleteTrackError(trackId);
        }

        AddDomainEvent(new TrackDeletedDomainEvent(trackId));
        return BaseResult.Success();
    }
}