using Microsoft.Extensions.DependencyInjection;

namespace RtfmAPI.Infrastructure.Dao;

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
    public static IServiceCollection AddDao(this IServiceCollection services)
    {
        // services.AddAutoMapper(typeof(BandsMappingProfile));

        return services;
    }
}