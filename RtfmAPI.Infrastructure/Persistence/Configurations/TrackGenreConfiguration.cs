using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;
using RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;
using RtfmAPI.Infrastructure.Dao.Dao.Tracks;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackGenreConfiguration : IEntityTypeConfiguration<TrackGenreDao>
{
    public void Configure(EntityTypeBuilder<TrackGenreDao> builder)
    {
        builder.ToTable("TrackGenre");

        builder.HasKey(trackGenre => new {trackGenre.TrackId, trackGenre.GenreId});

        builder
            .Property(trackGenre => trackGenre.TrackId)
            .ValueGeneratedNever();
        
        builder
            .Property(trackGenre => trackGenre.GenreId)
            .ValueGeneratedNever();

        builder
            .HasOne<TrackDao>()
            .WithMany()
            .HasForeignKey(trackGenre => trackGenre.TrackId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne<GenreDao>()
            .WithMany()
            .HasForeignKey(trackGenre => trackGenre.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}