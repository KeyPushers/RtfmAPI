using RtfmAPI.Domain.Exceptions;

namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal abstract class TrackFileException : DomainException
{
    internal TrackFileException(string message) : base($"{nameof(TrackFileException)}.{message}")
    {
    }
}