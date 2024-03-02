using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class GenresConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
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
            .HasConversion(entity => entity.Value,
                name => GenreName.Create(name).Value);
    }
}