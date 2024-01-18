namespace RftmAPI.Domain.Exceptions.BandExceptions;

/// <summary>
/// Исключения удаления музыкальной группы.
/// </summary>
public class DeleteBandException : BandException
{
    /// <summary>
    /// Создание исключения удаления музыкальной группы.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public DeleteBandException(string message) : base($"{nameof(DeleteBandException)}. {message}")
    {
    }
}