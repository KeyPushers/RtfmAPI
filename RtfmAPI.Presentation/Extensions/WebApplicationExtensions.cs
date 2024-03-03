using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtfmAPI.Infrastructure.Persistence.Context;
using RtfmAPI.Presentation.Settings;
using Serilog;

namespace RtfmAPI.Presentation.Extensions;

/// <summary>
/// Расширения <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Задание пользовательских настроек для инициализации <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/>.</param>
    /// <returns><see cref="WebApplication"/>.</returns>
    public static WebApplication UseWebApplicationExtension(this WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseOpenApi();
            webApplication.UseSwaggerUi();
            webApplication.UseReDoc(ReDocSettings.Apply);
        }

        webApplication.UseSerilogRequestLogging();
        webApplication.MapControllers();

        webApplication.Lifetime.ApplicationStarted.Register(OnApplicationStarted, webApplication.Services);
        
        return webApplication;
    }

    private static async void OnApplicationStarted(object? provider)
    {
        if (provider is not ServiceProvider serviceProvider)
        {
            return;
        }

        await InitDataBaseAsync(serviceProvider);
    }


    private static Task InitDataBaseAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataContext>();
        return context.InitAsync();
    }
}