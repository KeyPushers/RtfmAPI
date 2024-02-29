using System;
using System.Collections.Generic;
using RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;

namespace RtfmAPI.Infrastructure.Dao.Dao.Track;

public class TrackDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid? TrackFileId { get; set; }
    public Guid? AlbumId { get; set; }
    public List<TrackGenreDao> GenreIds { get; set; } = new();
}