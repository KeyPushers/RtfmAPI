using System;
using RtfmAPI.Infrastructure.Dao.Dao.Albums;
using RtfmAPI.Infrastructure.Dao.Dao.Bands;

namespace RtfmAPI.Infrastructure.Dao.Dao.BandAlbum;

public class BandAlbumDao
{
    public Guid BandId { get; set; }
    public BandDao Band { get; set; } = new();

    public Guid AlbumId { get; set; }
    public AlbumDao Album { get; set; } = new();
}