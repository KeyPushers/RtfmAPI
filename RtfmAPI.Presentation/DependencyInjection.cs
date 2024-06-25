using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtfmAPI.Presentation.Settings;

namespace RtfmAPI.Presentation;

/// <summary>
/// Класс определения зависимостей слоя "Презентация"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Приложение"
    /// </summary>
    public static IHostApplicationBuilder AddPresentation(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(OpenApiSettings.OpenApiDocument);

        return builder;
    }
}