using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.Entities;

/// <summary>
/// Представление данных файла музыкального трека.
/// </summary>
public class TrackFile : Entity<TrackFileId>
{
    /// <summary>
    /// Название файла.
    /// </summary>
    public TrackFileName Name { get; private set; }

    /// <summary>
    /// Расширение файла.
    /// </summary>
    public TrackFileExtension Extension { get; private set; }

    /// <summary>
    /// MIME-тип файла.
    /// </summary>
    public TrackFileMimeType MimeType { get; private set; }

    /// <summary>
    /// Содержимое файла.
    /// </summary>
    public TrackFileData Data { get; private set; }

    /// <summary>
    /// Объем файла в байтах.
    /// </summary>
    public int Size => Data.Value.Length;

    /// <summary>
    /// Создание представления данных файла музыкального трека.
    /// </summary>
#pragma warning disable CS8618
    private TrackFile()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание представления данных файла музыкального трека.
    /// </summary>
    /// <param name="name">Названия файла музыкального трека.</param>
    /// <param name="extension">Расширение файла музыкального трека.</param>
    /// <param name="mimeType">MIME-тип файла музыкального трека.</param>
    /// <param name="data">Содержимое файла музыкального трека.</param>
    private TrackFile(TrackFileName name, TrackFileExtension extension, TrackFileMimeType mimeType, TrackFileData data)
        : base(TrackFileId.Create())
    {
        Name = name;
        Extension = extension;
        MimeType = mimeType;
        Data = data;
    }

    /// <summary>
    /// Создание представления данных файла музыкального трека.
    /// </summary>
    /// <param name="name">Названия файла музыкального трека.</param>
    /// <param name="extension">Расширение файла музыкального трека.</param>
    /// <param name="mimeType">MIME-тип файла музыкального трека.</param>
    /// <param name="data">Содержимое файла музыкального трека.</param>
    /// <returns>Представление данных файла музыкального трека.</returns>
    public static Result<TrackFile> Create(TrackFileName name, TrackFileExtension extension,
        TrackFileMimeType mimeType,
        TrackFileData data)
    {
        return new TrackFile(name, extension, mimeType, data);
    }
}