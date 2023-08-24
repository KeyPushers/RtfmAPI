using Microsoft.EntityFrameworkCore;
using RtfmAPI.Infrastructure.Persistence.Configurations;

namespace RtfmAPI.Infrastructure.Persistence.Context;

/// <summary>
/// Основной контекст базы данных
/// </summary>
public sealed class AppDbContext : DbContext
{

    /// <summary>
    /// Основной контекст базы данных
    /// </summary>
    /// <param name="dbContextOptions">Настройки базы данных</param>
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    
    /// <summary>
    /// Подключаем конфигурации к контексту
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new TestConfiguration());
        modelBuilder.ApplyConfiguration(new TrackConfiguration());
        // modelBuilder.ApplyConfiguration(new AlbumConfiguration());
        // modelBuilder.ApplyConfiguration(new BandConfiguration());
        // modelBuilder.ApplyConfiguration(new GenreConfiguration());
    }
}