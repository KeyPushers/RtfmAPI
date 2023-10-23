using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурирование таблиц доменной модели <see cref="Album"/>.
/// </summary>
public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    /// <summary>
    /// Конфигурирование таблиц доменной модели <see cref="Album"/>.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        ConfigureAlbumsTable(builder);
        ConfigureAlbumTrackIdsTable(builder);
    }

    /// <summary>
    /// Конфигурирование таблицы "Albums".
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureAlbumsTable(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");
        
        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => AlbumId.Create(id));
        
        builder.Property(album => album.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Value,
                name => AlbumName.Create(name).ValueOrDefault);
    }

    /// <summary>
    /// Конфигурирование таблицы "AlbumTrackIds".
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureAlbumTrackIdsTable(EntityTypeBuilder<Album> builder)
    {
        builder.OwnsMany(h => h.TrackIds, dib =>
        {
            dib.WithOwner().HasForeignKey("AlbumId");
        
            dib.ToTable("AlbumTrackIds");
        
            dib.HasKey("Id");
        
            dib.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("AlbumTrackId");
        });
        
        builder.Metadata
            .FindNavigation(nameof(Album.TrackIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}