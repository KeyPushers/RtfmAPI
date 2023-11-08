using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Genres;

/// <summary>
/// Музыкальный жанр.
/// </summary>
public sealed class Genre : AggregateRoot<GenreId, Guid>
{
    // TODO: Сделать жанры независимыми: убрать список треков и музыкальных групп.
    private readonly List<TrackId> _trackIds = new();
    private readonly List<BandId> _bandIds = new();

    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    public GenreName Name { get; private set; }

    /// <summary>
    /// Музыкальные треки.
    /// </summary>
    public IReadOnlyCollection<TrackId> TrackIds => _trackIds.ToList();

    /// <summary>
    /// Музыкальные группы.
    /// </summary>
    public IReadOnlyCollection<BandId> BandIds => _bandIds.ToList();

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
#pragma warning disable CS8618
    private Genre()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    private Genre(GenreName name) : base(GenreId.Create())
    {
        Name = name;
    }

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    /// <param name="tracks">Музыкальные треки.</param>
    /// <param name="bands">Музыкальные группы.</param>
    private Genre(GenreName name, IEnumerable<Track> tracks, IEnumerable<Band> bands) : base(GenreId.Create())
    {
        Name = name;
        _trackIds = tracks.Select(track => (TrackId) track.Id).ToList();
        _bandIds = bands.Select(band => (BandId) band.Id).ToList();
    }

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    public static Result<Genre> Create(GenreName name)
    {
        return new Genre(name);
    }

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    /// <param name="tracks">Музыкальные треки.</param>
    /// <param name="bands">Музыкальные группы.</param>
    /// <returns>Музыкальный жанр.</returns>
    public static Result<Genre> Create(GenreName name, IEnumerable<Track> tracks, IEnumerable<Band> bands)
    {
        return new Genre(name, tracks, bands);
    }
}