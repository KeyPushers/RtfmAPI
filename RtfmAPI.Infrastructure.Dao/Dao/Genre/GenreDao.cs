using System;
using System.Collections.Generic;

namespace RtfmAPI.Infrastructure.Dao.Dao.Genre;

public class GenreDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<Guid> Tracks { get; set; } = new();
    public List<Guid> Bands { get; set; } = new();
}