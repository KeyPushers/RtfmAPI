using System;
using RtfmAPI.Domain.Models.TrackFiles.Events;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles;

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
    /// <param name="id">Идентификатор файла музыкального трека.</param>
    /// <param name="name">Название файла музыкального трека.</param>
    /// <param name="data">Содержимое файла музыкального трека.</param>
    /// <param name="extension">Расширение файла музыкального трека.</param>
    /// <param name="mimeType">MIME-тип файла музыкального трека.</param>
    /// <param name="duration">Продолжительность файла музыкального трека.</param>
    private TrackFile(TrackFileId id, TrackFileName name, TrackFileData data, TrackFileExtension extension,
        TrackFileMimeType mimeType, TrackFileDuration duration) : base(id)
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
    private TrackFile(TrackFileName name, TrackFileData data, TrackFileExtension extension, TrackFileMimeType mimeType,
        TrackFileDuration duration) : this(TrackFileId.Create(), name, data, extension, mimeType, duration)
    {
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
    internal static Result<TrackFile> Create(TrackFileName name, TrackFileData data,
        TrackFileExtension extension, TrackFileMimeType mimeType, TrackFileDuration duration)
    {
        var trackFile = new TrackFile(name, data, extension, mimeType, duration);
        trackFile.AddDomainEvent(new TrackFileCreatedDomainEvent(trackFile.Id));
        trackFile.AddDomainEvent(new TrackFileNameChangedDomainEvent(trackFile.Id, trackFile.Name));
        trackFile.AddDomainEvent(new TrackFileDataChangedDomainEvent(trackFile.Id, trackFile.Data));
        trackFile.AddDomainEvent(new TrackFileExtensionChangedDomainEvent(trackFile.Id, trackFile.Extension));
        trackFile.AddDomainEvent(new TrackFileMimeTypeChangedDomainEvent(trackFile.Id, trackFile.MimeType));
        trackFile.AddDomainEvent(new TrackFileDurationChangedDomainEvent(trackFile.Id, trackFile.Duration));

        return trackFile;
    }

    /// <summary>
    /// Восстановление файла музыкального трека.
    /// </summary>
    /// <param name="id">Идентификатор файла музыкального трека.</param>
    /// <param name="name">Название файла музыкального трека.</param>
    /// <param name="data">Содержимое файла музыкального трека.</param>
    /// <param name="extension">Расширение файла музыкального трека.</param>
    /// <param name="mimeType">MIME-тип файла музыкального трека.</param>
    /// <param name="duration">Продолжительность файла музыкального трека.</param>
    /// <returns>Файл музыкального трека.</returns>
    internal static Result<TrackFile> Restore(TrackFileId id, TrackFileName name, TrackFileData data,
        TrackFileExtension extension, TrackFileMimeType mimeType, TrackFileDuration duration)
    {
        return new TrackFile(id, name, data, extension, mimeType, duration);
    }

    /// <summary>
    /// Удаление файла музыкального трека.
    /// </summary>
    public BaseResult Delete()
    {
        AddDomainEvent(new TrackFileDeletedDomainEvent(Id));
        return BaseResult.Success();
    }
}