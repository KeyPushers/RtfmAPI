using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Albums;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        // Id
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id);

        // Name
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new AlbumName(name));
        
        // Release Date
        builder.Property(entity => entity.ReleaseDate);
        
        // BandId
        builder.Property(entity => entity.BandId);
        
        // Tracks
        builder.Property(entity => entity.Tracks);
    }
}