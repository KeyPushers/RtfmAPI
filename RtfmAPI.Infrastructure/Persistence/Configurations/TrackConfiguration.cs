using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Tracks;
using RftmAPI.Domain.Models.Tracks.ValueObjects;

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
        ConfigureTrackAlbumsIdsTable(builder);
    }
    
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
                name => TrackName.Create(name).Value);
    }
    
    private static void ConfigureTrackAlbumsIdsTable(EntityTypeBuilder<Track> builder)
    {
        builder.OwnsMany(track => track.AlbumIds, b =>
        {
            b.ToTable("TrackAlbumIds");
        
            b.WithOwner().HasForeignKey("TrackId");
            
            b.HasKey("Id");
        
            b.Property(albumId => albumId.Value)
                .ValueGeneratedNever()
                .HasColumnName("TrackAlbumId");
        });
        
        builder.Metadata
            .FindNavigation(nameof(Track.AlbumIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}