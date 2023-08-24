using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Genres;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // Id
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id);

        // Name
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new GenreName(name));
        
        // Tracks
        builder.Property(entity => entity.Tracks);
        
        // Bands
        builder.Property(entity => entity.Bands);
    }
}