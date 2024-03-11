using RtfmAPI.Domain.Exceptions;

namespace RtfmAPI.Domain.Models.Genres.Exceptions;

internal abstract class GenreException : DomainException
{
    internal GenreException(string message) : base($"{nameof(GenreException)}.{message}")
    {
    }
}