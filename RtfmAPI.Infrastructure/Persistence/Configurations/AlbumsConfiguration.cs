using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class AlbumsConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        // Определение названия таблицы музыкальных альбомов.
        builder.ToTable("Albums");

        // Определение идентификатора музыкального альбома.
        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => AlbumId.Create(id));

        // Определение названия музыкального альбома.
        builder.Property(album => album.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Value,
                name => AlbumName.Create(name).Value);

        // Определение даты выпуска музыкального альбома.
        builder
            .Property(track => track.ReleaseDate)
            .HasConversion(entity => entity.Value,
                name => AlbumReleaseDate.Create(name).Value);
        
        // Создание таблицы для связи музыкальных альбомов и музыкальных треков.
        builder.OwnsMany(h => h.TrackIds, albumTrackIdsBuilder =>
        {
            // Определение названия таблицы связи музыкальных альбомов и музыкальных треков.
            albumTrackIdsBuilder.ToTable("AlbumTrackIds");

            // Определение идентификатора музыкального альбома в музыкальном треке.
            albumTrackIdsBuilder.WithOwner().HasForeignKey("AlbumId");

            // Определение идентификатора таблицы.
            albumTrackIdsBuilder.HasKey("Id");

            // Определение идентификатора музыкального трека в таблице "AlbumTrackIds".
            albumTrackIdsBuilder.Property(trackId => trackId.Value)
                .ValueGeneratedNever()
                .HasColumnName("AlbumTrackId");
        });

        builder.Metadata
            .FindNavigation(nameof(Album.TrackIds))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}