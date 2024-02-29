using System;
using System.Collections.Generic;
using RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;

namespace RtfmAPI.Infrastructure.Dao.Dao.Genre;

public class GenreDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<TrackGenreDao> TrackIds { get; set; } = new();
}