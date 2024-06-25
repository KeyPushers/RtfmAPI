namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal class TrackFileDataException : TrackFileException
{
    internal TrackFileDataException(string message) : base($"{nameof(TrackFileDataException)}.{message}")
    {
    }
}