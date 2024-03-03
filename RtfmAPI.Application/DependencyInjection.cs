using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RtfmAPI.Application.Fabrics;
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
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        
        services.AddScoped<BandsFabric>();
        services.AddScoped<AlbumsFabric>();

        return services;
    }
}