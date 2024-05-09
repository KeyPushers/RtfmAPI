﻿using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;
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
        options.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
            In = OpenApiSecurityApiKeyLocation.Header,
            Type = OpenApiSecuritySchemeType.ApiKey,
            Scheme = "Bearer"
            
        });
        options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
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