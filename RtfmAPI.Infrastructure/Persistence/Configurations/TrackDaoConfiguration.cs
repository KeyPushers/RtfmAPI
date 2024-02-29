using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Album;
using RtfmAPI.Infrastructure.Dao.Dao.Track;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFile;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackDaoConfiguration : IEntityTypeConfiguration<TrackDao>
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

        builder
            .HasOne<AlbumDao>()
            .WithMany()
            .HasForeignKey(track => track.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}