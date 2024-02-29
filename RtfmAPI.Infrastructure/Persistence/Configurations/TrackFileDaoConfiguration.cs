using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackFileDaoConfiguration : IEntityTypeConfiguration<TrackFileDao>
{
    public void Configure(EntityTypeBuilder<TrackFileDao> builder)
    {
        // Определение названия таблицы музыкальных треков.
        builder.ToTable("TrackFiles");

        // Определение идентификатора музыкального трека.
        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever();

        // Определение названия музыкального трека.
        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackFileName.MaxLength);

        // Определение содержимое файла музыкального трека.
        builder.Property(trackFile => trackFile.Data);

        // Определение расширения файла музыкального трека.
        builder.Property(trackFile => trackFile.Extension);

        // Определение MIME-типа файла музыкального трека.
        builder.Property(trackFile => trackFile.MimeType)
            .HasMaxLength(TrackFileName.MaxLength);

        // Определение продолжительности файла музыкального трека.
        builder
            .Property(trackFile => trackFile.Duration)
            .ValueGeneratedNever();
    }
}