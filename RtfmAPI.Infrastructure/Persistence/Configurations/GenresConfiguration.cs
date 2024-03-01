using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.BandGenre;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;
using RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;


public class GenresConfiguration : IEntityTypeConfiguration<GenreDao>
{
    public void Configure(EntityTypeBuilder<GenreDao> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(genre => genre.Id);
        builder.Property(genre => genre.Id)
            .ValueGeneratedNever();
        
        builder.Property(genre => genre.Name);
        
        builder
            .HasMany<BandGenreDao>()
            .WithOne()
            .HasForeignKey(bandGenre => bandGenre.GenreId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder
            .HasMany<TrackGenreDao>()
            .WithOne()
            .HasForeignKey(trackGenre => trackGenre.GenreId)
            .OnDelete(DeleteBehavior.SetNull);
    }
    
}