using System.IO;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;

/// <summary>
/// Объект переноса данных файла музыкального трека.
/// </summary>
public class TrackFile
{
    /// <summary>
    /// Файл музыкального трека.
    /// </summary>
    public MemoryStream? File { get; init; }

    /// <summary>
    /// Название файла музыкального трека.
    /// </summary>
    public string? FileName { get; init; }

    /// <summary>
    /// Расширение файла музыкального трека.
    /// </summary>
    public string? Extension { get; init; }

    /// <summary>
    /// MIME-тип файла музыкального трека.
    /// </summary>
    public string? MimeType { get; init; }
}