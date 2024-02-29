using System;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;
using RtfmAPI.Infrastructure.Dao.Dao.Track;

namespace RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;

public class TrackGenreDao
{
    public Guid TrackId { get; set; }
    public TrackDao Track { get; set; } = new();

    public Guid GenreId { get; set; }
    public GenreDao Genre { get; set; } = new();
}