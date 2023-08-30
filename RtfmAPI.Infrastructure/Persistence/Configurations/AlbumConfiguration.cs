using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        ConfigureAlbumsTable(builder);
        ConfigureTracksAlbumsTable(builder);
    }
    
    private static void ConfigureAlbumsTable(EntityTypeBuilder<Album> builder)
    {
        // Id
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value, id => AlbumId.Create(id));

        // Name
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new AlbumName(name));
        
        // Release Date
        builder.Property(entity => entity.ReleaseDate);
    }
    
    private static void ConfigureTracksAlbumsTable(EntityTypeBuilder<Album> builder)
    {
        builder.OwnsMany(track => track.TrackIds, albums =>
        {
            albums.WithOwner().HasForeignKey("TrackId");
            
            albums.HasKey("Id");
            albums.Property("Id");
        });
    }
}