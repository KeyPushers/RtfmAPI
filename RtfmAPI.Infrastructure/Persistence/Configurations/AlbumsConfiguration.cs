using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.Albums;
using RtfmAPI.Infrastructure.Dao.Dao.BandAlbum;
using RtfmAPI.Infrastructure.Dao.Dao.Tracks;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class AlbumsConfiguration : IEntityTypeConfiguration<AlbumDao>
{
    public void Configure(EntityTypeBuilder<AlbumDao> builder)
    {
        builder.ToTable("Albums");

        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id).ValueGeneratedNever();

        builder.Property(album => album.Name);

        builder.Property(album => album.ReleaseDate);

        builder
            .HasMany<TrackDao>()
            .WithOne()
            .HasForeignKey(track => track.AlbumId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Many-To-One for BandAlbumDao and AlbumDao
        builder
            .HasMany<BandAlbumDao>()
            .WithOne()
            .HasForeignKey(bandAlbum => bandAlbum.BandId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}