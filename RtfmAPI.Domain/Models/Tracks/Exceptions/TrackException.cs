using RtfmAPI.Domain.Exceptions;

namespace RtfmAPI.Domain.Models.Tracks.Exceptions;

internal abstract class TrackException : DomainException
{
    protected TrackException(string message) : base($"{nameof(TrackException)}.{message}")
    {
    }
}