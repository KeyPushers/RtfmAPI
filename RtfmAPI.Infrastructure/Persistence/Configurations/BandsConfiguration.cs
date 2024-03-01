using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.BandAlbum;
using RtfmAPI.Infrastructure.Dao.Dao.BandGenre;
using RtfmAPI.Infrastructure.Dao.Dao.Bands;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class BandsConfiguration : IEntityTypeConfiguration<BandDao>
{
    public void Configure(EntityTypeBuilder<BandDao> builder)
    {
        builder.ToTable("Bands");

        builder.HasKey(band => band.Id);
        builder.Property(band => band.Id).ValueGeneratedNever();

        builder.Property(band => band.Name);
        
        builder
            .HasMany<BandGenreDao>()
            .WithOne()
            .HasForeignKey(brandGenre => brandGenre.BandId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Many-To-One for BandAlbumDao and BandDao
        builder
            .HasMany<BandAlbumDao>()
            .WithOne()
            .HasForeignKey(bandAlbum => bandAlbum.AlbumId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}