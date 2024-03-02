using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Bands;
using RftmAPI.Domain.Models.Bands.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class BandsConfiguration : IEntityTypeConfiguration<Band>
{
    public void Configure(EntityTypeBuilder<Band> builder)
    {
        // Определение названия таблицы музыкальных групп.
        builder.ToTable("Bands");

        // Определение идентификатора музыкальной группы.
        builder.HasKey(band => band.Id);
        builder.Property(band => band.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => BandId.Create(id));

        // Определение названия музыкальной группы.
        builder.Property(album => album.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Value,
                name => BandName.Create(name).Value);
        
        // Создание таблицы для связи музыкальных группы и музыкальных альбомов.
        builder.OwnsMany(h => h.AlbumIds, dib =>
        {
            // Определение названия таблицы связи музыкальных группы и музыкальных альбомов.
            dib.ToTable("BandAlbumIds");

            // Определение идентификатора музыкальной группы в музыкальном альбоме.
            dib.WithOwner().HasForeignKey("BandId");

            // Определение идентификатора таблицы.
            dib.HasKey("Id");

            // Определение идентификатора музыкального альбома в таблице "BandAlbumIds".
            dib.Property(albumId => albumId.Value)
                .ValueGeneratedNever()
                .HasColumnName("BandAlbumIds");
        });

        builder.Metadata
            .FindNavigation(nameof(Band.AlbumIds))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        
        // Создание таблицы для связи музыкальных группы и музыкальных жанров.
        builder.OwnsMany(h => h.GenreIds, dib =>
        {
            // Определение названия таблицы связи музыкальных группы и музыкальных жанров.
            dib.ToTable("BandGenreIds");

            // Определение идентификатора музыкальной группы в музыкальном жанре.
            dib.WithOwner().HasForeignKey("BandId");

            // Определение идентификатора таблицы.
            dib.HasKey("Id");

            // Определение идентификатора музыкального жанра в таблице "BandGenreIds".
            dib.Property(genreId => genreId.Value)
                .ValueGeneratedNever()
                .HasColumnName("BandGenreId");
        });

        builder.Metadata
            .FindNavigation(nameof(Band.GenreIds))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}