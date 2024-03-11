namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal class TrackFileExtensionException : TrackFileException
{
    internal TrackFileExtensionException(string message) : base($"{nameof(TrackFileExtensionException)}.{message}")
    {
    }
}