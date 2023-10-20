using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Bands.ValueObjects;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Bands;

/// <summary>
/// Музыкальная группа.
/// </summary>
public sealed class Band : AggregateRoot<BandId, Guid>
{
    private readonly List<AlbumId> _albumIds;
    private readonly List<GenreId> _genreIds;

    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public BandName Name { get; private set; }

    /// <summary>
    /// Музыкальные альбомы.
    /// </summary>
    public IReadOnlyCollection<AlbumId> AlbumIds => _albumIds;

    /// <summary>
    /// Музыкальные жанры.
    /// </summary>
    public IReadOnlyCollection<GenreId> GenreIds => _genreIds;

    /// <summary>
    /// Музыкальная группа.
    /// </summary>
#pragma warning disable CS8618
    private Band()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Музыкальная группа.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public Band(BandName name) : base(BandId.Create())
    {
        Name = name;

        _albumIds = new();
        _genreIds = new();
    }
}