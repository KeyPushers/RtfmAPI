namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal class TrackFileNameException : TrackFileException
{
    internal TrackFileNameException(string message) : base($"{nameof(TrackFileNameException)}.{message}")
    {
    }
}