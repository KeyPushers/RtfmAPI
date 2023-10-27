using Microsoft.AspNetCore.Builder;
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
    public static WebApplication UseWebApplicationExtensions(this WebApplication webApplication)
    {
        webApplication.UseSerilogRequestLogging();

        return webApplication;
    }
}