using NSwag.Generation.AspNetCore;
using OpenApiContact = NSwag.OpenApiContact;
using OpenApiInfo = NSwag.OpenApiInfo;

namespace RtfmAPI.Presentation.Settings;

/// <summary>
/// Настройка данных и описания страницы Swagger.
/// </summary>
public static class OpenApiSettings
{
    /// <summary>
    /// Получение настроек.
    /// </summary>
    public static void OpenApiDocument(AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        options.PostProcess = PostProcess;
    }
    
    /// <summary>
    /// Обработка.
    /// </summary>
    /// <param name="document"><see cref="NSwag.OpenApiDocument "/>.</param>
    private static void PostProcess(NSwag.OpenApiDocument document)
    {
        document.Info = new OpenApiInfo
        {
            Title = "RtfmAPI",
            Description = "Проект прослушивания и передачи аудио.",
            TermsOfService = null,
            Contact = new OpenApiContact
            {
                Name = "tg",
                Url = "https://t.me/+EJ4XyQRqOdZiNTRi"
            },
            Version = "0.0.1"
        };
    }
}