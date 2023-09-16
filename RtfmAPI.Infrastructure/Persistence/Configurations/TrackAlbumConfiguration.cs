using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;
using RftmAPI.Domain.DomainNeeds.TrackAlbum;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackAlbumConfiguration : IEntityTypeConfiguration<TrackAlbum>
{
    public void Configure(EntityTypeBuilder<TrackAlbum> builder)
    {
        ConfigureTrackAlbumTable(builder);
    }

    private void ConfigureTrackAlbumTable(EntityTypeBuilder<TrackAlbum> builder)
    {
        builder.ToTable("TrackAlbum");
        
        builder.HasKey(trackAlbum => trackAlbum.Id);
        builder.Property(trackAlbum => trackAlbum.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => TrackAlbumId.Create(value));
        
        builder.Property(trackAlbum => trackAlbum.TrackId)
            .ValueGeneratedNever()
            .HasConversion(trackId => trackId.Value, value => TrackId.Create(value));

        builder
            .HasOne<Track>()
            .WithMany()
            .HasForeignKey(trackAlbum => trackAlbum.TrackId);
        // builder.Property(trackAlbum => trackAlbum.AlbumId)
        //     .ValueGeneratedNever()
        //     .HasConversion(albumId => albumId.Value, value => AlbumId.Create(value));
    }
}