using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;
using RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackGenreDaoConfiguration : IEntityTypeConfiguration<TrackGenreDao>
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
            .HasOne(trackGenre => trackGenre.Track)
            .WithMany(track => track.GenreIds)
            .HasForeignKey(trackGenre => trackGenre.TrackId);
        
        builder
            .HasOne(trackGenre => trackGenre.Genre)
            .WithMany(track => track.TrackIds)
            .HasForeignKey(trackGenre => trackGenre.GenreId);
    }
    
}