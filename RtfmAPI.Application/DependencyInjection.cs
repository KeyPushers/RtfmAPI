using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtfmAPI.Application.PipelineBehaviors;

namespace RtfmAPI.Application;

/// <summary>
/// Класс определения зависимостей слоя "Приложение"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Приложение"
    /// </summary>
    /// <returns>Коллекция сервисов</returns>
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        
        return builder;
    }
}