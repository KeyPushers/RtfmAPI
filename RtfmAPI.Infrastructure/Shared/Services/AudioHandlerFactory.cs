using Microsoft.Extensions.Logging;
using RtfmAPI.Application.Common.Interfaces.AudioHandlers;

namespace RtfmAPI.Infrastructure.Shared.Services;

/// <summary>
/// Создание фабрики аудио обработчика.
/// </summary>
public class AudioHandlerFactory : IAudioHandlerFactory
{
    private readonly ILogger<AudioHandlerFactory> _logger;

    /// <summary>
    /// Создание фабрики аудио обработчика.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public AudioHandlerFactory(ILogger<AudioHandlerFactory> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// <inheritdoc cref="IAudioHandlerFactory.Create"/>
    /// </summary>
    /// <param name="stream">Поток аудиофайла.</param>
    /// <param name="mimetype">MIME-тип файла.</param>
    /// <returns>Интерфейс обработчика аудиофайла.</returns>
    public IAudioHandler? Create(Stream stream, string mimetype)
    {
        try
        {
            return new TagLibSharpService(stream, mimetype);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при создании сервиса {nameof(TagLibSharpService)}");
            return null;
        }
    }
}