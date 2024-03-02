using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Dao.Dao.Albums;

public class AlbumDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Guid> TrackIds { get; set; } = new();
}