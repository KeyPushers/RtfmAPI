using System;

namespace RtfmAPI.Infrastructure.Dao.Dao.Tracks;

public class TrackDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid? TrackFileId { get; set; }
    // public string? AlbumId { get; set; }
    // public List<string> GenreIds { get; set; } = new();
}