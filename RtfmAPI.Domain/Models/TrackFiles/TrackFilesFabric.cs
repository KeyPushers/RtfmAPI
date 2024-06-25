using System;
using RtfmAPI.Domain.Fabrics;
using RtfmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.TrackFiles;

/// <summary>
/// Фабрика доменной модели файла музыкального трека.
/// </summary>
public class TrackFilesFabric : AggregateFabric<TrackFile, TrackFileId, Guid>
{
    private readonly string _name;
    private readonly byte[] _data;
    private readonly string _extension;
    private readonly string _mimeType;
    private readonly double _duration;

    /// <summary>
    /// Созданеи фабрики.
    /// </summary>
    public TrackFilesFabric(string name, byte[] data, string extension, string mimeType, double duration)
    {
        _name = name;
        _data = data;
        _extension = extension;
        _mimeType = mimeType;
        _duration = duration;
    }

    /// <inheritdoc />
    public override Result<TrackFile> Create()
    {
        var trackFileNameResult = TrackFileName.Create(_name);
        if (trackFileNameResult.IsFailed)
        {
            return trackFileNameResult.Error;
        }

        var trackFileDataResult = TrackFileData.Create(_data);
        if (trackFileDataResult.IsFailed)
        {
            return trackFileDataResult.Error;
        }

        var trackFileExtensionResult = TrackFileExtension.Create(_extension);
        if (trackFileExtensionResult.IsFailed)
        {
            return trackFileExtensionResult.Error;
        }

        var trackFileMimeTypeResult = TrackFileMimeType.Create(_mimeType);
        if (trackFileMimeTypeResult.IsFailed)
        {
            return trackFileMimeTypeResult.Error;
        }

        var trackFileDurationResult = TrackFileDuration.Create(_duration);
        if (trackFileDurationResult.IsFailed)
        {
            return trackFileDurationResult.Error;
        }

        return TrackFile.Create(trackFileNameResult.Value, trackFileDataResult.Value, trackFileExtensionResult.Value,
            trackFileMimeTypeResult.Value, trackFileDurationResult.Value);
    }

    /// <inheritdoc />
    public override Result<TrackFile> Restore(Guid id)
    {
        var trackFileId = TrackFileId.Create(id);

        var trackFileNameResult = TrackFileName.Create(_name);
        if (trackFileNameResult.IsFailed)
        {
            return trackFileNameResult.Error;
        }

        var trackFileDataResult = TrackFileData.Create(_data);
        if (trackFileDataResult.IsFailed)
        {
            return trackFileDataResult.Error;
        }

        var trackFileExtensionResult = TrackFileExtension.Create(_extension);
        if (trackFileExtensionResult.IsFailed)
        {
            return trackFileExtensionResult.Error;
        }

        var trackFileMimeTypeResult = TrackFileMimeType.Create(_mimeType);
        if (trackFileMimeTypeResult.IsFailed)
        {
            return trackFileMimeTypeResult.Error;
        }

        var trackFileDurationResult = TrackFileDuration.Create(_duration);
        if (trackFileDurationResult.IsFailed)
        {
            return trackFileDurationResult.Error;
        }

        return TrackFile.Restore(trackFileId, trackFileNameResult.Value, trackFileDataResult.Value,
            trackFileExtensionResult.Value,
            trackFileMimeTypeResult.Value, trackFileDurationResult.Value);
    }
}