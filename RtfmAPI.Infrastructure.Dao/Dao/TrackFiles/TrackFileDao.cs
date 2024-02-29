using System;

namespace RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;

public class TrackFileDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public string? Extension { get; set; }
    public string? MimeType { get; set; }
    public double Duration { get; set; }
}