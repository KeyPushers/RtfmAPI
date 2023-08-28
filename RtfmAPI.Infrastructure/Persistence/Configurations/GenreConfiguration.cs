using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Genres;
using RftmAPI.Domain.Aggregates.Genres.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // Id
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value, id => GenreId.Create(id));

        // Name
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new GenreName(name));
    }
}