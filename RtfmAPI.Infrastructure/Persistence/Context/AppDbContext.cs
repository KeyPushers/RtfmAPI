using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Infrastructure.Persistence.Configurations;
using RtfmAPI.Infrastructure.Persistence.Interceptors;

namespace RtfmAPI.Infrastructure.Persistence.Context;

/// <summary>
/// Основной контекст базы данных
/// </summary>
public sealed class AppDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    /// <summary>
    /// Основной контекст базы данных
    /// </summary>
    /// <param name="dbContextOptions">Настройки базы данных: <see cref="DbContextOptions{AppDbContext}"/>.</param>
    /// <param name="publishDomainEventsInterceptor">Перехватчик доменных событий: <see cref="PublishDomainEventsInterceptor"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(dbContextOptions)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    /// <summary>
    /// Подключаем конфигурации к контексту
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // IgnoreDomainEventsInDataBase(modelBuilder);
        modelBuilder.ApplyConfiguration(new TracksConfiguration());
        modelBuilder.ApplyConfiguration(new TrackFilesConfiguration());
        modelBuilder.ApplyConfiguration(new GenresConfiguration());
        modelBuilder.ApplyConfiguration(new AlbumsConfiguration());
        modelBuilder.ApplyConfiguration(new BandsConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Обработчик события изменения конфигурации.
    /// </summary>
    /// <param name="optionsBuilder">Конструктор.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    /// Исключить доменные события из базы данных.
    /// </summary>
    /// <param name="modelBuilder">Конструктор.</param>
    private static void IgnoreDomainEventsInDataBase(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}