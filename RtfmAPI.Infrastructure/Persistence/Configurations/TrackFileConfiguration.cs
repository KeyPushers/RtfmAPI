using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.TrackFiles;
using RftmAPI.Domain.Models.TrackFiles.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация файла музыкального трека.
/// </summary>
public class TrackFileConfiguration : IEntityTypeConfiguration<TrackFile>
{
    /// <summary>
    /// Конфигурирование таблиц доменной модели <see cref="TrackFile"/>.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    public void Configure(EntityTypeBuilder<TrackFile> builder)
    {
        ConfigureTrackFilesTable(builder);
    }

    /// <summary>
    /// Создание таблицы файлов музыкальных треков.
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="NotImplementedException"></exception>
    private static void ConfigureTrackFilesTable(EntityTypeBuilder<TrackFile> builder)
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
    }
}