using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурирование таблицы доменной модели <see cref="Genre"/>.
/// </summary>
public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    /// <summary>
    /// Конфигурирование таблицы доменной модели <see cref="Genre"/>.
    /// </summary>
    /// <param name="builder">Конструктор</param>
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        ConfigureGenresTable(builder);
        ConfigureGenreTrackIdsTable(builder);
        ConfigureGenreBandIdsTable(builder);
    }

    /// <summary>
    /// Создание таблицы музыкальных жанров.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private void ConfigureGenresTable(EntityTypeBuilder<Genre> builder)
    {
        // Определение названия таблицы музыкальных жанров.
        builder.ToTable("Genres");

        // Определение идентификатора музыкального жанра.
        builder.HasKey(genre => genre.Id);
        builder.Property(genre => genre.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => GenreId.Create(id));

        // Определение названия музыкального жанра.
        builder.Property(genre => genre.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Value,
                name => GenreName.Create(name).Value);
    }

    /// <summary>
    /// Создание таблицы для связи музыкальных жанров и музыкальных треков.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private void ConfigureGenreTrackIdsTable(EntityTypeBuilder<Genre> builder)
    {
        // Создание таблицы для связи музыкальных жанров и музыкальных треков.
        builder.OwnsMany(h => h.TrackIds, dib =>
        {
            // Определение названия таблицы связи музыкальных жанров и музыкальных треков.
            dib.ToTable("GenreTrackIds");

            // Определение идентификатора музыкального жанра в музыкальном треке.
            dib.WithOwner().HasForeignKey("GenreId");

            // Определение идентификатора таблицы.
            dib.HasKey("Id");

            // Определение идентификатора музыкального трека в таблице "GenreTrackIds".
            dib.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("GenreTrackId");
        });

        builder.Metadata
            .FindNavigation(nameof(Genre.TrackIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    /// <summary>
    /// Создание таблицы для связи музыкальных жанров и музыкальных групп.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private void ConfigureGenreBandIdsTable(EntityTypeBuilder<Genre> builder)
    {
        // Создание таблицы для связи музыкальных жанров и музыкальных групп.
        builder.OwnsMany(h => h.BandIds, dib =>
        {
            // Определение названия таблицы связи музыкальных жанров и музыкальных групп.
            dib.ToTable("GenreBandIds");

            // Определение идентификатора музыкального жанра в музыкальной группе.
            dib.WithOwner().HasForeignKey("GenreId");

            // Определение идентификатора таблицы.
            dib.HasKey("Id");

            // Определение идентификатора музыкального трека в таблице "GenreBandIds".
            dib.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("GenreBandId");
        });

        builder.Metadata
            .FindNavigation(nameof(Genre.BandIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}