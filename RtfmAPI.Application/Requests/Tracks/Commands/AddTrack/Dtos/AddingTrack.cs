using System.IO;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;

/// <summary>
/// Объект переноса данных добавляемого музыкального трека.
/// </summary>
public class AddingTrack
{
    /// <summary>
    /// Создание объекта переноса данных добавляемого музыкального трека.
    /// </summary>
    /// <param name="memoryStream">Потока файла музыкального трека.</param>
    public AddingTrack(MemoryStream memoryStream)
    {
        File = memoryStream;
    }
    
    /// <summary>
    /// Файл музыкального трека.
    /// </summary>
    public MemoryStream File { get; }

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