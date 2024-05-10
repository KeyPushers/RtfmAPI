using FluentResults;
using RtfmAPI.Application.Fabrics.TrackFiles.Daos;
using RtfmAPI.Domain.Models.TrackFiles;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Application.Fabrics.TrackFiles;

/// <summary>
/// Фабрика файлов музыкальных треков.
/// </summary>
public class TrackFilesFactory : IEntityFactory<TrackFile, TrackFileId>
{
    private readonly TrackFileDao _trackFileDao;

    /// <summary>
    /// Создание фабрики файлов музыкальных треков.
    /// </summary>
    /// <param name="trackFileDao">Объект доступа данных файла музыкального трека.</param>
    public TrackFilesFactory(TrackFileDao trackFileDao)
    {
        _trackFileDao = trackFileDao;
    }

    /// <inheritdoc/>
    public Result<TrackFile> Create()
    {
        var trackFileNameResult = TrackFileName.Create(_trackFileDao.Name ?? string.Empty);
        if (trackFileNameResult.IsFailed)
        {
            return trackFileNameResult.ToResult();
        }

        var trackFileName = trackFileNameResult.ValueOrDefault;

        var trackFileDataResult = TrackFileData.Create(_trackFileDao.Data);
        if (trackFileDataResult.IsFailed)
        {
            return trackFileDataResult.ToResult();
        }

        var trackFileData = trackFileDataResult.ValueOrDefault;

        var trackFileExtensionResult = TrackFileExtension.Create(_trackFileDao.Extension ?? string.Empty);
        if (trackFileExtensionResult.IsFailed)
        {
            return trackFileExtensionResult.ToResult();
        }

        var trackFileExtension = trackFileExtensionResult.ValueOrDefault;

        var trackFileMimeTypeResult = TrackFileMimeType.Create(_trackFileDao.MimeType ?? string.Empty);
        if (trackFileMimeTypeResult.IsFailed)
        {
            return trackFileMimeTypeResult.ToResult();
        }

        var trackFileMimeType = trackFileMimeTypeResult.ValueOrDefault;

        var trackFileDurationResult = TrackFileDuration.Create(_trackFileDao.Duration);
        if (trackFileDurationResult.IsFailed)
        {
            return trackFileDurationResult.ToResult();
        }

        var trackFileDuration = trackFileDurationResult.ValueOrDefault;

        return TrackFile.Create(trackFileName, trackFileData, trackFileExtension, trackFileMimeType, trackFileDuration);
    }

    /// <inheritdoc/>
    public Result<TrackFile> Restore()
    {
        var trackFileId = TrackFileId.Create(_trackFileDao.Id);

        var trackFileNameResult = TrackFileName.Create(_trackFileDao.Name ?? string.Empty);
        if (trackFileNameResult.IsFailed)
        {
            return trackFileNameResult.ToResult();
        }

        var trackFileName = trackFileNameResult.ValueOrDefault;

        var trackFileDataResult = TrackFileData.Create(_trackFileDao.Data);
        if (trackFileDataResult.IsFailed)
        {
            return trackFileDataResult.ToResult();
        }

        var trackFileData = trackFileDataResult.ValueOrDefault;

        var trackFileExtensionResult = TrackFileExtension.Create(_trackFileDao.Extension ?? string.Empty);
        if (trackFileExtensionResult.IsFailed)
        {
            return trackFileExtensionResult.ToResult();
        }

        var trackFileExtension = trackFileExtensionResult.ValueOrDefault;

        var trackFileMimeTypeResult = TrackFileMimeType.Create(_trackFileDao.MimeType ?? string.Empty);
        if (trackFileMimeTypeResult.IsFailed)
        {
            return trackFileMimeTypeResult.ToResult();
        }

        var trackFileMimeType = trackFileMimeTypeResult.ValueOrDefault;

        var trackFileDurationResult = TrackFileDuration.Create(_trackFileDao.Duration);
        if (trackFileDurationResult.IsFailed)
        {
            return trackFileDurationResult.ToResult();
        }

        var trackFileDuration = trackFileDurationResult.ValueOrDefault;

        return TrackFile.Restore(trackFileId, trackFileName, trackFileData, trackFileExtension, trackFileMimeType,
            trackFileDuration);
    }
}