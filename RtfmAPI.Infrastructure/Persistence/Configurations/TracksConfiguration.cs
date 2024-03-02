using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Albums;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;
using RtfmAPI.Infrastructure.Dao.Dao.Tracks;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TracksConfiguration : IEntityTypeConfiguration<TrackDao>
{
    public void Configure(EntityTypeBuilder<TrackDao> builder)
    {
        builder.ToTable("Tracks");

        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever();

        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackName.MaxLength);

        builder.Property(track => track.ReleaseDate);
        
        builder
            .HasOne<TrackFileDao>()
            .WithOne()
            .HasForeignKey<TrackDao>(track => track.TrackFileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}