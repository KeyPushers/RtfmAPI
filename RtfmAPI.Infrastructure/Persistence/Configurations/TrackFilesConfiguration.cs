using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;
using RtfmAPI.Infrastructure.Dao.Dao.Tracks;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackFilesConfiguration : IEntityTypeConfiguration<TrackFileDao>
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
        
        // builder
        //     .HasOne<TrackDao>()
        //     .WithOne()
        //     .HasForeignKey<TrackFileDao>("TrackId")
        //     .OnDelete(DeleteBehavior.SetNull);
    }
}