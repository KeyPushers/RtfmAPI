using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RftmAPI.Domain.Aggregates.Albums.Repository;
using RftmAPI.Domain.Aggregates.Tracks.Repository;
using RtfmAPI.Infrastructure.Persistence.Context;
using RtfmAPI.Infrastructure.Persistence.Repositories;

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
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestingDataBase"));

        services.AddScoped<ITracksRepository, TracksRepository>();
        services.AddScoped<IAlbumsRepository, AlbumsRepository>();

        return services;
    }
}