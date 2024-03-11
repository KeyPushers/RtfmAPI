namespace RtfmAPI.Domain.Models.Albums.Exceptions;

internal class AlbumNameException : AlbumException
{
    public AlbumNameException(string message) : base($"{nameof(AlbumNameException)}.{message}")
    {
    }
}