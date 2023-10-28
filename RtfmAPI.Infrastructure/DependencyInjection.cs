﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RftmAPI.Domain.Models.Albums.Repository;
using RftmAPI.Domain.Models.Bands.Repository;
using RftmAPI.Domain.Models.TrackFiles.Repository;
using RftmAPI.Domain.Models.Tracks.Repository;
using RftmAPI.Domain.Utils;
using RtfmAPI.Infrastructure.Persistence.Context;
using RtfmAPI.Infrastructure.Persistence.Repositories;

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
        services.AddInMemoryDatabase();
        // services.AddPostgresDatabase();
        
        services.AddScoped<ITracksRepository, TracksRepository>();
        services.AddScoped<ITrackFilesRepository, TrackFilesRepository>();
        services.AddScoped<IAlbumsRepository, AlbumsRepository>();
        services.AddScoped<IBandsRepository, BandsRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    /// <summary>
    /// Использование базы данных в памяти
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    private static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestingDataBase"));

        return services;
    }
    
    /// <summary>
    /// Использование базы данных Postgres
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    private static IServiceCollection AddPostgresDatabase(this IServiceCollection services)
    {
        var sqlConnectionBuilder = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = "Host=localhost;Port=5432;Database=RtfmDb;UserId=postgres;Password=admin"
        };

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(sqlConnectionBuilder.ConnectionString));
        
        return services;
    }
}