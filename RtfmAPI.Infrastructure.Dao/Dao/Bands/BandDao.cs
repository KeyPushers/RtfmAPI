using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Dao.Dao.Bands;

public class BandDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<Guid> AlbumIds { get; set; } = new();
    public List<Guid> GenreIds { get; set; } = new();
}