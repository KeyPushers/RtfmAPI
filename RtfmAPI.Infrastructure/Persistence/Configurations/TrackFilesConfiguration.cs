using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackFilesConfiguration : IEntityTypeConfiguration<TrackFile>
{
    public void Configure(EntityTypeBuilder<TrackFile> builder)
    {
        // Определение названия таблицы музыкальных треков.
        builder.ToTable("TrackFiles");

        // Определение идентификатора музыкального трека.
        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackFileId.Create(value));

        // Определение названия музыкального трека.
        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackFileName.MaxLength)
            .HasConversion(entity => entity.Value,
                name => TrackFileName.Create(name).Value);
        
        // Определение содержимое файла музыкального трека.
        builder.Property(trackFile => trackFile.Data)
            .HasConversion(entity => entity.Value,
                value => TrackFileData.Create(value).Value);
        
        // Определение расширения файла музыкального трека.
        builder.Property(trackFile => trackFile.Extension)
            .HasConversion(entity => entity.Value,
                value => TrackFileExtension.Create(value).Value);

        // Определение MIME-типа файла музыкального трека.
        builder.Property(trackFile => trackFile.MimeType)
            .HasConversion(entity => entity.Value,
                value => TrackFileMimeType.Create(value).Value)
            .HasMaxLength(TrackFileName.MaxLength);
        
        // Определение продолжительности файла музыкального трека.
        builder
            .Property(trackFile => trackFile.Duration)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackFileDuration.Create(value).Value);
    }
}