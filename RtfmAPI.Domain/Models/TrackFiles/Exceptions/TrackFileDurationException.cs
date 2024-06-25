namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal class TrackFileDurationException : TrackFileException
{
    internal TrackFileDurationException(string message) : base($"{nameof(TrackFileDurationException)}.{message}")
    {
    }
}