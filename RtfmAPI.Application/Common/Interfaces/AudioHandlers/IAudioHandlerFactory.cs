using System.IO;

namespace RtfmAPI.Application.Common.Interfaces.AudioHandlers;

/// <summary>
/// Интерфейс фабрики обработчика аудиофайлов.
/// </summary>
public interface IAudioHandlerFactory
{
    /// <summary>
    /// Создание обработчика аудиофайла.
    /// </summary>
    /// <param name="stream">Поток аудиофайла.</param>
    /// <param name="mimetype">MIME-тип файла.</param>
    /// <returns>Интерфейс обработчика аудиофайла.</returns>
    IAudioHandler? Create(Stream stream, string mimetype);
}