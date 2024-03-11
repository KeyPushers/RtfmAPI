namespace RtfmAPI.Domain.Models.Tracks.Exceptions;

internal class TrackNameException : TrackException
{
    public TrackNameException(string message) : base($"{nameof(TrackNameException)}.{message}")
    {
    }
}