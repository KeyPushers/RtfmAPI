using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.Albums;
using RtfmAPI.Infrastructure.Dao.Dao.BandAlbum;
using RtfmAPI.Infrastructure.Dao.Dao.Bands;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class BandAlbumConfiguration : IEntityTypeConfiguration<BandAlbumDao>
{
    public void Configure(EntityTypeBuilder<BandAlbumDao> builder)
    {
        builder.ToTable("BandAlbum");

        builder.HasKey(bandAlbum => new {bandAlbum.BandId, bandAlbum.AlbumId});
        
        // One-To-Many for BandDao and BandAlbumDao
        builder
            .HasOne<BandDao>()
            .WithMany()
            .HasForeignKey(bandAlbum => bandAlbum.BandId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One-To-Many for AlbumDao and BandAlbumDao
        builder
            .HasOne<AlbumDao>()
            .WithMany()
            .HasForeignKey(bandAlbum => bandAlbum.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}