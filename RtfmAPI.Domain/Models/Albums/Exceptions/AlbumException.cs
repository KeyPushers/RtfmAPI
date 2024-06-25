using RtfmAPI.Domain.Exceptions;

namespace RtfmAPI.Domain.Models.Albums.Exceptions;

internal abstract class AlbumException : DomainException
{
    protected AlbumException(string message) : base($"{nameof(AlbumException)}.{message}")
    {
    }
}