using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Tracks;

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
        // Id
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id);

        // Name
        builder
            .Property(entity => entity.Name)
            .HasMaxLength(TrackName.NameMaxLength)
            .HasConversion(entity => entity.Name,
                name => new TrackName(name));

        // Data
        builder.Property(entity => entity.Data);

        // Release Date
        builder.Property(entity => entity.ReleaseDate);
        
        // Genres
        builder.HasMany(entity => entity.Genres)
            .WithOne(entity => entity.Value)
            .HasConversion(entity => entity.);

        // Albums
        builder.Property(entity => entity.Albums);
    }
}