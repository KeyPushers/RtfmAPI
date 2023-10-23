using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.Entities;
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
    /// Конфигурирование таблицы "Tracks".
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTracksTable(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("Tracks");

        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => TrackId.Create(value));

        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackName.MaxLength)
            .HasConversion(entity => entity.Value,
                name => TrackName.Create(name).ValueOrDefault);

        builder
            .Property(track => track.ReleaseDate)
            .HasConversion(entity => entity.Value,
                name => TrackReleaseDate.Create(name).ValueOrDefault);
        
        builder
            .Property(track => track.AlbumId)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value,
                value => AlbumId.Create(value));
    }

    /// <summary>
    /// Конфигурирование таблицы "TrackTrackFile".
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTrackTrackFilesTable(EntityTypeBuilder<Track> builder)
    {
        builder.OwnsOne(track => track.TrackFile, b =>
        {
            b.ToTable("TrackTrackFiles");

            b.WithOwner().HasForeignKey("TrackId");

            b.HasKey("Id");

            b.Property(trackFile => trackFile.Id)
                .ValueGeneratedNever()
                .HasConversion(entity => entity.Value,
                    value => TrackFileId.Create(value));

            b.Property(trackFile => trackFile.Name)
                .HasConversion(entity => entity.Value,
                    value => TrackFileName.Create(value).ValueOrDefault)
                .HasMaxLength(TrackFileName.MaxLength);
            
            b.Property(trackFile => trackFile.Extension)
                .HasConversion(entity => entity.Value,
                    value => TrackFileExtension.Create(value).ValueOrDefault)
                .HasMaxLength(TrackFileName.MaxLength);
            
            b.Property(trackFile => trackFile.MimeType)
                .HasConversion(entity => entity.Value,
                    value => TrackFileMimeType.Create(value).ValueOrDefault)
                .HasMaxLength(TrackFileName.MaxLength);
            
            b.Property(trackFile => trackFile.Data)
                .HasConversion(entity => entity.Value,
                    value => TrackFileData.Create(value).ValueOrDefault)
                .HasMaxLength(TrackFileName.MaxLength);
        });

        builder.Metadata
            .FindNavigation(nameof(Track.TrackFile))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    /// <summary>
    /// Конфигурирование таблицы "TrackGenreIds".
    /// </summary>
    /// <param name="builder">Конструктор.</param>
    private static void ConfigureTrackGenreIdsTable(EntityTypeBuilder<Track> builder)
    {
        builder.OwnsMany(track => track.GenreIds, b =>
        {
            b.ToTable("TrackGenreIds");

            b.WithOwner().HasForeignKey("TrackId");

            b.HasKey("Id");

            b.Property(albumId => albumId.Value)
                .ValueGeneratedNever()
                .HasColumnName("TrackGenreId");
        });

        builder.Metadata
            .FindNavigation(nameof(Track.GenreIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}