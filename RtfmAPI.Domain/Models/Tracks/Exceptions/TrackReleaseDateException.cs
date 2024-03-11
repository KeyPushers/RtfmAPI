namespace RtfmAPI.Domain.Models.Tracks.Exceptions;

internal class TrackReleaseDateException : TrackException
{
    public TrackReleaseDateException(string message) : base(
        $"{nameof(TrackReleaseDateException)}.{message}")
    {
    }
}