using RftmAPI.Domain.Exceptions.TrackFileExceptions;
using RftmAPI.Domain.Models.TrackFiles.Events;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.TrackFiles;

/// <summary>
/// Файл музыкального трека.
/// </summary>
public sealed class TrackFile : AggregateRoot<TrackFileId, Guid>
{
    /// <summary>
    /// Название файла музыкального трека.
    /// </summary>
    public TrackFileName Name { get; private set; }

    /// <summary>
    /// Содержимое файла музыкального трека.
    /// </summary>
    public TrackFileData Data { get; private set; }

    /// <summary>
    /// Расширение файла музыкального трека.
    /// </summary>
    public TrackFileExtension Extension { get; private set; }

    /// <summary>
    /// MIME-тип файла музыкального трека.
    /// </summary>
    public TrackFileMimeType MimeType { get; private set; }

    /// <summary>
    /// Продолжительность файла музыкального трека.
    /// </summary>
    public TrackFileDuration Duration { get; private set; }
    
    /// <summary>
    /// Создание файла музыкального трека.
    /// </summary>
#pragma warning disable CS8618
    private TrackFile(TrackFileDuration duration)
    {
        Duration = duration;
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание файла музыкального трека.
    /// </summary>
    /// <param name="name">Название файла музыкального трека.</param>
    /// <param name="data">Содержимое файла музыкального трека.</param>
    /// <param name="extension">Расширение файла музыкального трека.</param>
    /// <param name="mimeType">MIME-тип файла музыкального трека.</param>
    /// <param name="duration">Продолжительность файла музыкального трека.</param>
    private TrackFile(TrackFileName name, TrackFileData data, TrackFileExtension extension, TrackFileMimeType mimeType, TrackFileDuration duration)
        : base(TrackFileId.Create())
    {
        Name = name;
        Data = data;
        Extension = extension;
        MimeType = mimeType;
        Duration = duration;
    }

    /// <summary>
    /// Создание файла музыкального трека.
    /// </summary>
    /// <param name="name">Название файла музыкального трека.</param>
    /// <param name="data">Содержимое файла музыкального трека.</param>
    /// <param name="extension">Расширение файла музыкального трека.</param>
    /// <param name="mimeType">MIME-тип файла музыкального трека.</param>
    /// <param name="duration">Продолжительность файла музыкального трека.</param>
    /// <returns>Файл музыкального трека.</returns>
    public static Result<TrackFile> Create(TrackFileName name, TrackFileData data,
        TrackFileExtension extension, TrackFileMimeType mimeType, TrackFileDuration duration)
    {
        return new TrackFile(name, data, extension, mimeType, duration);
    }

    /// <summary>
    /// Удаление файла музыкального трека.
    /// </summary>
    /// <param name="deleteAction">Делегат, отвечающий за удаление файла музыкального трека.</param>
    public async Task<BaseResult> DeleteAsync(Func<TrackFile, Task<bool>> deleteAction)
    {
        var trackFileId = (TrackFileId) Id;
        var deleteActionResult = await deleteAction(this);
        if (!deleteActionResult)
        {
            return TrackFileExceptions.DeleteTrackError(trackFileId);
        }

        AddDomainEvent(new TrackFileDeletedDomainEvent(trackFileId));
        return BaseResult.Success();
    }
}