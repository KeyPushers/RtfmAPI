using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Dao.Dao.Tracks;

public class TrackDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid? TrackFileId { get; set; }
    public List<Guid> GenreIds { get; set; } = new();
}