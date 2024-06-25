using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtfmAPI.Infrastructure.Data;

namespace RtfmAPI.Presentation.Extensions;

/// <summary>
/// Настройка опций.
/// </summary>
public static class OptionsConfiguration
{
    /// <summary>
    /// Настройка опций
    /// </summary>
    public static IHostApplicationBuilder ConfigureOptions(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        var dbSettingsOptions = builder.Configuration.GetSection(DbSettingsOptions.DbSettings);

        services.Configure<DbSettingsOptions>(dbSettingsOptions);
        
        return builder;
    }
}