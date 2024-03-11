namespace RtfmAPI.Domain.Models.Albums.Exceptions;

internal class AlbumReleaseDateException : AlbumException
{
    public AlbumReleaseDateException(string message) : base($"{nameof(AlbumReleaseDateException)}.{message}")
    {
    }
}