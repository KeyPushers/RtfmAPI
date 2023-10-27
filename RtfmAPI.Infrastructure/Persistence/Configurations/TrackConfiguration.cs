using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация музыкального трека
/// </summary>
internal class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    /// <summary>
    /// Конфигурирование таблиц доменной модели <see cref="Track"/>.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        ConfigureTracksTable(builder);
        ConfigureTrackTrackFilesTable(builder);
        ConfigureTrackGenreIdsTable(builder);
    }

    /// <summary>
    /// Создание таблицы музыкальных треков.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTracksTable(EntityTypeBuilder<Track> builder)
    {
        // Определение названия таблицы музыкальных треков.
        builder.ToTable("Tracks");

        // Определение идентификатора музыкального трека.
        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackId.Create(value));

        // Определение названия музыкального трека.
        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackName.MaxLength)
            .HasConversion(entity => entity.Value,
                name => TrackName.Create(name).Value);

        // Определение даты выпуска музыкального трека.
        builder
            .Property(track => track.ReleaseDate)
            .HasConversion(entity => entity.Value,
                name => TrackReleaseDate.Create(name).Value);

        // Определение идентификатора музыкального альбома.
        builder
            .Property(track => track.AlbumId)
            .ValueGeneratedNever()
            .HasConversion(entity => entity!.Value,
                value => AlbumId.Create(value));
    }

    /// <summary>
    /// Создание таблицы для связи музыкального трека и файла музыкального трека.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTrackTrackFilesTable(EntityTypeBuilder<Track> builder)
    {
        // Создание таблицы для связи музыкального трека и файла музыкального трека.
        builder.OwnsOne(track => track.TrackFile, b =>
        {
            // Определение названия таблицы связи музыкального трека и файла музыкального трека.
            b.ToTable("TrackTrackFiles");

            // Определение идентификатора музыкального трека в файле музыкального трека..
            b.WithOwner().HasForeignKey("TrackId");

            // Определение идентификатора таблицы.
            b.HasKey("Id");
            
            // Определение идентификатора файла музыкального трека.
            b.Property(trackFile => trackFile.Id)
                .ValueGeneratedNever()
                .HasConversion(entity => entity.Value,
                    value => TrackFileId.Create(value));

            // Определение названия файла музыкального трека.
            b.Property(trackFile => trackFile.Name)
                .HasConversion(entity => entity.Value,
                    value => TrackFileName.Create(value).Value)
                .HasMaxLength(TrackFileName.MaxLength);

            // Определение расширения файла музыкального трека.
            b.Property(trackFile => trackFile.Extension)
                .HasConversion(entity => entity.Value,
                    value => TrackFileExtension.Create(value).Value)
                .HasMaxLength(TrackFileName.MaxLength);

            // Определение MIME-типа файла музыкального трека.
            b.Property(trackFile => trackFile.MimeType)
                .HasConversion(entity => entity.Value,
                    value => TrackFileMimeType.Create(value).Value)
                .HasMaxLength(TrackFileName.MaxLength);

            // Определение содержимое файла музыкального трека.
            b.Property(trackFile => trackFile.Data)
                .HasConversion(entity => entity.Value,
                    value => TrackFileData.Create(value).Value)
                .HasMaxLength(TrackFileName.MaxLength);
        });

        builder.Metadata
            .FindNavigation(nameof(Track.TrackFile))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    /// <summary>
    /// Создание таблицы для связи музыкальных треков и музыкальных жанров.
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTrackGenreIdsTable(EntityTypeBuilder<Track> builder)
    {
        // Создание таблицы для связи музыкальных треков и музыкальных жанров.
        builder.OwnsMany(track => track.GenreIds, b =>
        {
            // Определение названия таблицы связи музыкальных треков и музыкального жанра.
            b.ToTable("TrackGenreIds");

            // Определение идентификатора таблицы.
            b.HasKey("Id");

            // Определение идентификатора музыкального трека в музыкальном жанре.
            b.WithOwner().HasForeignKey("TrackId");
            
            // Определение идентификатора музыкального жанра в таблице "TrackGenreIds". 
            b.Property(albumId => albumId.Value)
                .ValueGeneratedNever()
                .HasColumnName("TrackGenreId");
        });

        builder.Metadata
            .FindNavigation(nameof(Track.GenreIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}