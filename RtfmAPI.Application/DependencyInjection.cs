using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RtfmAPI.Application.PipelineBehaviors;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Genres;

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
        
        // TODO Перенести на доменный слой!
        services.AddScoped<BandsFabric>();
        services.AddScoped<AlbumsFabric>();
        services.AddScoped<GenresFabric>();

        return services;
    }
}