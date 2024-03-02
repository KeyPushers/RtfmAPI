using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
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

        return webApplication;
    }
}