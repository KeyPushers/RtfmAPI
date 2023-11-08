using System.IO;

namespace RtfmAPI.Application.Requests.Tracks.Queries.GetTrackStream.Dtos;

/// <summary>
/// Объекта переноса данных потока музыкального трека.
/// </summary>
public sealed class TrackStream
{
    /// <summary>
    /// Поток файла музыкального трека.
    /// </summary>
    public Stream Stream { get; }

    /// <summary>
    /// MIME-тип.
    /// </summary>
    public string MediaType { get; }
    
    /// <summary>
    /// Создание объекта переноса данных потока музыкального трека.
    /// </summary>
    /// <param name="stream">Поток файла музыкального трека.</param>
    /// <param name="mediaType">MIME-тип.</param>
    public TrackStream(Stream stream, string mediaType)
    {
        Stream = stream;
        MediaType = mediaType;
    }
}