namespace RtfmAPI.Application.Interfaces.AudioHandlers;

/// <summary>
/// Интерфейс обработчика аудиофайла.
/// </summary>
public interface IAudioHandler
{
    /// <summary>
    /// Получение продолжительности аудиофайла.
    /// </summary>
    /// <returns>Длина аудиофайла в миллисекундах.</returns>
    double GetDuration();
}