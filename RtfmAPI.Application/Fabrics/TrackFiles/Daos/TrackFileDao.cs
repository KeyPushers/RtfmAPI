using System;

namespace RtfmAPI.Application.Fabrics.TrackFiles.Daos;

/// <summary>
/// Объект доступа данных файла музыкального трека.
/// </summary>
public class TrackFileDao
{
    /// <summary>
    /// Идентификатор файла музыкального трека.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// название файла музыкального трека.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Содержимое файла музыкального трека.
    /// </summary>
    public byte[] Data { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Расширение файла музыкального трека.
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    /// MIME-тип файла музыкального трека.
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Продолжительность файла музыкального трека.
    /// </summary>
    public double Duration { get; set; }
}