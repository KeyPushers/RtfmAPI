using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        ConfigureId(builder);
        ConfigureName(builder);
        ConfigureReleaseDate(builder);

        ConfigureTracksAlbumsTable(builder);
    }
    
    private static void ConfigureId(EntityTypeBuilder<Album> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value, id => AlbumId.Create(id));
    }

    private static void ConfigureName(EntityTypeBuilder<Album> builder)
    {
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new AlbumName(name));
    }

    private static void ConfigureReleaseDate(EntityTypeBuilder<Album> builder)
    {
        builder.Property(entity => entity.ReleaseDate);
    }

    private static void ConfigureTracksAlbumsTable(EntityTypeBuilder<Album> builder)
    {
        // builder.OwnsMany(track => track.TrackIds, albums =>
        // {
        //     albums.WithOwner().HasForeignKey("TrackId");
        //     
        //     albums.HasKey("Id");
        //     albums.Property("Id");
        // });
    }
}