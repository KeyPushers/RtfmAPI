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
}