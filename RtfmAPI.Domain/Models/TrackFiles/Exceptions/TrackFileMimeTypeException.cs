namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal class TrackFileMimeTypeException : TrackFileException
{
    internal TrackFileMimeTypeException(string message) : base($"{nameof(TrackFileMimeTypeException)}.{message}")
    {
    }
}