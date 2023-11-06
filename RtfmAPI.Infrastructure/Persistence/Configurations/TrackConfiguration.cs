using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация музыкального трека.
/// </summary>
internal class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    /// <summary>
    /// Конфигурирование таблиц доменной модели <see cref="Track"/>.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        ConfigureTracksTable(builder);
        ConfigureTrackGenreIdsTable(builder);
    }

    /// <summary>
    /// Создание таблицы музыкальных треков.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTracksTable(EntityTypeBuilder<Track> builder)
    {
        // Определение названия таблицы музыкальных треков.
        builder.ToTable("Tracks");

        // Определение идентификатора музыкального трека.
        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackId.Create(value));

        // Определение названия музыкального трека.
        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackName.MaxLength)
            .HasConversion(entity => entity.Value,
                name => TrackName.Create(name).Value);

        // Определение даты выпуска музыкального трека.
        builder
            .Property(track => track.ReleaseDate)
            .HasConversion(entity => entity.Value,
                name => TrackReleaseDate.Create(name).Value);

        // Определение идентификатора музыкального альбома.
        builder
            .Property(track => track.AlbumId)
            .ValueGeneratedNever()
            .HasConversion(entity => entity!.Value,
                value => AlbumId.Create(value));
        
        // Определение продолжительности музыкального трека.
        builder
            .Property(track => track.Duration)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackDuration.Create(value).Value);
        
        // Определение идентификатора файла музыкального трека.
        builder
            .Property(track => track.TrackFileId)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackFileId.Create(value));
    }
    
    /// <summary>
    /// Создание таблицы для связи музыкальных треков и музыкальных жанров.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTrackGenreIdsTable(EntityTypeBuilder<Track> builder)
    {
        // Создание таблицы для связи музыкальных треков и музыкальных жанров.
        builder.OwnsMany(track => track.GenreIds, b =>
        {
            // Определение названия таблицы связи музыкальных треков и музыкального жанра.
            b.ToTable("TrackGenreIds");

            // Определение идентификатора таблицы.
            b.HasKey("Id");

            // Определение идентификатора музыкального трека в музыкальном жанре.
            b.WithOwner().HasForeignKey("TrackId");
            
            // Определение идентификатора музыкального жанра в таблице "TrackGenreIds". 
            b.Property(albumId => albumId.Value)
                .ValueGeneratedNever()
                .HasColumnName("TrackGenreId");
        });

        builder.Metadata
            .FindNavigation(nameof(Track.GenreIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}