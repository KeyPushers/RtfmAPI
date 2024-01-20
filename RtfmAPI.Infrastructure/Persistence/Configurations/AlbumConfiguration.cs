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
        ConfigureAlbumBandIdsTable(builder);
    }

    /// <summary>
    /// Создание таблицы музыкальных альбомов.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureAlbumsTable(EntityTypeBuilder<Album> builder)
    {
        // Определение названия таблицы музыкальных альбомов.
        builder.ToTable("Albums");

        // Определение идентификатора музыкального альбома.
        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => AlbumId.Create(id));

        // Определение названия музыкального альбома.
        builder.Property(album => album.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Value,
                name => AlbumName.Create(name).Value);

        // Определение даты выпуска музыкального альбома.
        builder
            .Property(track => track.ReleaseDate)
            .HasConversion(entity => entity.Value,
                name => AlbumReleaseDate.Create(name).Value);
    }

    /// <summary>
    /// Создание таблицы для связи музыкальных альбомов и музыкальных треков.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureAlbumTrackIdsTable(EntityTypeBuilder<Album> builder)
    {
        // Создание таблицы для связи музыкальных альбомов и музыкальных треков.
        builder.OwnsMany(h => h.TrackIds, albumTrackIdsBuilder =>
        {
            // Определение названия таблицы связи музыкальных альбомов и музыкальных треков.
            albumTrackIdsBuilder.ToTable("AlbumTrackIds");

            // Определение идентификатора музыкального альбома в музыкальном треке.
            albumTrackIdsBuilder.WithOwner().HasForeignKey("AlbumId");

            // Определение идентификатора таблицы.
            albumTrackIdsBuilder.HasKey("Id");

            // Определение идентификатора музыкального трека в таблице "AlbumTrackIds".
            albumTrackIdsBuilder.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("AlbumTrackId");
        });

        builder.Metadata
            .FindNavigation(nameof(Album.TrackIds))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    /// <summary>
    /// Создание таблицы для связи музыкальных альбомов и музыкальных треков.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureAlbumBandIdsTable(EntityTypeBuilder<Album> builder)
    {
        // Создание таблицы для связи музыкальных альбомов и музыкальных групп.
        builder.OwnsMany(h => h.BandIds, albumBandIdsBuilder =>
        {
            // Определение названия таблицы связи музыкальных альбомов и музыкальных групп.
            albumBandIdsBuilder.ToTable("AlbumBandIds");

            // Определение идентификатора музыкального альбома в музыкальной группе.
            albumBandIdsBuilder.WithOwner().HasForeignKey("AlbumId");

            // Определение идентификатора таблицы.
            albumBandIdsBuilder.HasKey("Id");

            // Определение идентификатора музыкальной группы в таблице "AlbumTrackIds".
            albumBandIdsBuilder.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("AlbumBandId");
        });

        builder.Metadata
            .FindNavigation(nameof(Album.BandIds))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}