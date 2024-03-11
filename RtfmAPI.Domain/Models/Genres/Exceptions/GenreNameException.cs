namespace RtfmAPI.Domain.Models.Genres.Exceptions;

internal class GenreNameException : GenreException
{
    internal GenreNameException(string message) : base($"{nameof(GenreNameException)}.{message}")
    {
    }
}