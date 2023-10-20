using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Genres;

/// <summary>
/// Музыкальный жанр.
/// </summary>
public sealed class Genre : AggregateRoot<GenreId, Guid>
{
    private readonly List<TrackId> _trackIds;
    private readonly List<BandId> _bandIds;

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
    /// Музыкальный жанр.
    /// </summary>
#pragma warning disable CS8618
    private Genre()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальный жанр.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    public Genre(GenreName name) : base(GenreId.Create())
    {
        Name = name;

        _trackIds = new();
        _bandIds = new();
    }
}