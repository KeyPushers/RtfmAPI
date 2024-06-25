using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtfmAPI.Application.Interfaces.AudioHandlers;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Interfaces.Persistence.Queries;
using RtfmAPI.Infrastructure.Persistence.Context;
using RtfmAPI.Infrastructure.Persistence.Repositories.Commands;
using RtfmAPI.Infrastructure.Persistence.Repositories.Queries;
using RtfmAPI.Infrastructure.Shared.Services;

namespace RtfmAPI.Infrastructure;

/// <summary>
/// Класс определения зависимостей слоя "Инфраструктура"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Инфраструктура"
    /// </summary>
    /// <returns>Коллекция сервисов</returns>
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddSingleton<DataContext>();

        services.AddScoped<IBandsCommandsRepository, BandsCommandsRepository>();
        services.AddScoped<IBandsQueriesRepository, BandsQueriesRepository>();

        services.AddScoped<IAlbumsCommandsRepository, AlbumsCommandsRepository>();
        services.AddScoped<IAlbumsQueriesRepository, AlbumsQueriesRepository>();

        services.AddScoped<IGenresCommandsRepository, GenreCommandsRepository>();
        services.AddScoped<IGenresQueriesRepository, GenresQueriesRepository>();

        services.AddScoped<ITrackFilesCommandsRepository, TrackFilesCommandsRepository>();
        services.AddScoped<ITrackFilesQueriesRepository, TrackFilesQueriesRepository>();

        services.AddScoped<ITracksCommandsRepository, TracksCommandsRepository>();
        services.AddScoped<ITracksQueriesRepository, TracksQueriesRepository>();

        services.AddScoped<IAudioHandlerFactory, AudioHandlerFactory>();

        return builder;
    }
}