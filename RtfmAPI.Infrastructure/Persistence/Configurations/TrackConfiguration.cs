using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        ConfigureId(builder);
        ConfigureName(builder);
        ConfigureReleaseDate(builder);

        ConfigureAlbums(builder);
    }


    private static void ConfigureId(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value, value => TrackId.Create(value));
    }

    private static void ConfigureName(EntityTypeBuilder<Track> builder)
    {
        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackName.NameMaxLength)
            .HasConversion(entity => entity.Name,
                name => new TrackName(name));
    }

    private static void ConfigureReleaseDate(EntityTypeBuilder<Track> builder)
    {
        builder.Property(track => track.ReleaseDate);
    }

    private static void ConfigureAlbums(EntityTypeBuilder<Track> builder)
    {
        // builder
        //     .HasMany(track => track.AlbumIds)
        //     .WithMany()
        //     .UsingEntity<TrackAlbum>(
        //         right => right.HasOne<AlbumId>().WithMany().HasForeignKey(e => e.AlbumId),
        //         left => left.HasOne<Track>().WithMany().HasForeignKey(e => e.AlbumId),
        //         entity =>
        //         {
        //             entity.ToTable("TrackAlbum");
        //             entity.HasKey(ur => new {ur.TrackId, ur.AlbumId});
        //         });
        
        // builder
        //     .HasMany(track => track.AlbumIds)
        //     .WithMany()
        //     .UsingEntity<TrackAlbum>(
        //         right => right.HasOne<AlbumId>().WithMany(),
        //         left => left.HasOne<Track>().WithMany(),
        //         entity =>
        //         {
        //             entity.ToTable("TrackAlbum");
        //             entity.HasKey(ur => new {ur.TrackId, ur.AlbumId});
        //         });
        
        // builder.OwnsMany(track => track.AlbumIds, albums =>
        // {
        //     albums.WithOwner().HasForeignKey("AlbumId");
        //     
        //     albums.HasKey("Id");
        //     albums.Property("Id");
        // });
    }
}