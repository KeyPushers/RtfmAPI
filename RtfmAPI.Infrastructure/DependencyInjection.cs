using Microsoft.Extensions.DependencyInjection;

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
        return services;
    }
}