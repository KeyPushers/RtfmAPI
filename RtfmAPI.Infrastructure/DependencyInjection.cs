using Microsoft.Extensions.DependencyInjection;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Infrastructure.Persistence.Context;
using RtfmAPI.Infrastructure.Persistence.Repositories.Commands;
using RtfmAPI.Infrastructure.Persistence.Repositories.Queries;

namespace RtfmAPI.Infrastructure;

/// <summary>
/// Класс определения зависимостей слоя "Инфраструктура"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Инфраструктура"
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DataContext>();
        
        services.AddScoped<IBandsCommandsRepository, BandsCommandsRepository>();
        services.AddScoped<IBandsQueriesRepository, BandsQueriesRepository>();
        
        services.AddScoped<IAlbumsCommandsRepository, AlbumsCommandsRepository>();
        services.AddScoped<IAlbumsQueriesRepository, AlbumsQueriesRepository>();
        
        services.AddScoped<IGenresQueriesRepository, GenresQueriesRepository>();
        services.AddScoped<IGenresCommandsRepository, GenreCommandsRepository>();
        
        return services;
    }
}