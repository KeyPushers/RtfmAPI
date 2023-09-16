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
        ConfigureAlbumTracksIds(builder);
    }

    private static void ConfigureAlbumsTable(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");
        
        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => AlbumId.Create(id));
        
        builder.Property(album => album.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new AlbumName(name));
    }

    
    private static void ConfigureAlbumTracksIds(EntityTypeBuilder<Album> builder)
    {
        builder.OwnsMany(h => h.TrackIds, dib =>
        {
            dib.WithOwner().HasForeignKey("AlbumId");
        
            dib.ToTable("AlbumTracksIds");
        
            dib.HasKey("Id");
        
            dib.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("TrackId");
        });
        
        builder.Metadata
            .FindNavigation(nameof(Album.TrackIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}