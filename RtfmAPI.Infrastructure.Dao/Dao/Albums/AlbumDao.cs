using System;
using System.Collections.Generic;
using RtfmAPI.Infrastructure.Dao.Dao.BandAlbum;

namespace RtfmAPI.Infrastructure.Dao.Dao.Albums;

public class AlbumDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Guid> TrackIds { get; set; } = new();
    public List<Guid> BandIds { get; set; } = new();
    // public List<BandAlbumDao> BandAlbums { get; set; } = new();
}