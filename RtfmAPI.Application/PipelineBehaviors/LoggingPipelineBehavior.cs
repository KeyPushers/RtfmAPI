using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.Logging;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Properties;

namespace RtfmAPI.Application.PipelineBehaviors;

/// <summary>
/// Конвейер логирования.
/// </summary>
/// <typeparam name="TRequest">Запрос.</typeparam>
/// <typeparam name="TResponse">Ответ.</typeparam>
public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result<TResponse>
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Создание конвейера логирования.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Обработчик конвейра логирования.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="next">Делегат запроса</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        var startingRequestMessage = string.Format(Resources.LoggingPipelineBehaviorStartingRequestInformation,
            typeof(TRequest).Name, DateTime.UtcNow);
        _logger.LogInformation("{Message}", startingRequestMessage);

        var result = await next();
        if (result.IsFailed)
        {
            var failureRequestMessage = string.Format(Resources.LoggingPipelineBehaviorCompletedRequestFailureError,
                typeof(TRequest).Name, result.Errors, DateTime.UtcNow);
            _logger.LogError("{Message}", failureRequestMessage);
        }

        var completedRequestMessage = string.Format(Resources.LoggingPipelineBehaviorCompletedRequestInformation,
            typeof(TRequest).Name, DateTime.UtcNow);
        _logger.LogInformation("{Message}", completedRequestMessage);

        return result;
    }
}