using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Track;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFile;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackFileDaoConfiguration : IEntityTypeConfiguration<TrackFileDao>
{
    public void Configure(EntityTypeBuilder<TrackFileDao> builder)
    {
        builder.ToTable("TrackFiles");

        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever();

        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackFileName.MaxLength);

        builder.Property(trackFile => trackFile.Data);

        builder.Property(trackFile => trackFile.Extension);

        builder.Property(trackFile => trackFile.MimeType)
            .HasMaxLength(TrackFileName.MaxLength);

        builder
            .Property(trackFile => trackFile.Duration)
            .ValueGeneratedNever();
        
        builder
            .HasOne<TrackDao>()
            .WithOne()
            .HasForeignKey<TrackFileDao>(track => track.Track)
            .OnDelete(DeleteBehavior.SetNull);
    }
}