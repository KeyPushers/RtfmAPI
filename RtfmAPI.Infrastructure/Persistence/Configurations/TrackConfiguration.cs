using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Albums;
using RftmAPI.Domain.Aggregates.Albums.ValueObjects;
using RftmAPI.Domain.Aggregates.Tracks;
using RftmAPI.Domain.Aggregates.Tracks.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация музыкального трека
/// </summary>
internal class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    /// <summary>
    /// Конфигурация
    /// </summary>
    /// <param name="builder">Билдер</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        ConfigureTracksTable(builder);
        ConfigureTracksAlbumsTable(builder);
    }

    private static void ConfigureTracksTable(EntityTypeBuilder<Track> builder)
    {
        ConfigureId(builder);
        ConfigureName(builder);
        ConfigureData(builder);
        ConfigureReleaseDate(builder);
    }

    private static void ConfigureTracksAlbumsTable(EntityTypeBuilder<Track> builder)
    {
        ConfigureAlbums(builder);
    }

    private static void ConfigureId(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value, value => TrackId.Create(value));
    }

    private static void ConfigureName(EntityTypeBuilder<Track> builder)
    {
        builder
            .Property(entity => entity.Name)
            .HasMaxLength(TrackName.NameMaxLength)
            .HasConversion(entity => entity.Name,
                name => new TrackName(name));
    }

    private static void ConfigureData(EntityTypeBuilder<Track> builder)
    {
        builder.Property(entity => entity.Data);
    }

    private static void ConfigureReleaseDate(EntityTypeBuilder<Track> builder)
    {
        builder.Property(entity => entity.ReleaseDate);
    }

    private static void ConfigureAlbums(EntityTypeBuilder<Track> builder)
    {
        builder.OwnsMany(track => track.AlbumIds, albums =>
        {
            albums.WithOwner().HasForeignKey("AlbumId");
            
            albums.HasKey("Id");
            albums.Property("Id");
        });
    }
}