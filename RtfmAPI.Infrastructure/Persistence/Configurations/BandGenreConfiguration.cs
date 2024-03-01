using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.BandGenre;
using RtfmAPI.Infrastructure.Dao.Dao.Bands;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class BandGenreConfiguration : IEntityTypeConfiguration<BandGenreDao>
{
    public void Configure(EntityTypeBuilder<BandGenreDao> builder)
    {
        builder.ToTable("BandGenre");

        builder.HasKey(bandGenre => new {bandGenre.BandId, bandGenre.GenreId});

        builder
            .Property(bandGenre => bandGenre.BandId)
            .ValueGeneratedNever();
        
        builder
            .Property(bandGenre => bandGenre.GenreId)
            .ValueGeneratedNever();

        builder
            .HasOne<BandDao>()
            .WithMany()
            .HasForeignKey(bandAlbum => bandAlbum.BandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<GenreDao>()
            .WithMany()
            .HasForeignKey(bandGenre => bandGenre.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}