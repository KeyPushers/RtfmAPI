﻿using System;

namespace RtfmAPI.Infrastructure.Daos;

public class AlbumDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime ReleaseDate { get; set; }
}