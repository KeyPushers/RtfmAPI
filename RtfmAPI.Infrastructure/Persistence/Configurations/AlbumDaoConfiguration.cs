using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.Album;
using RtfmAPI.Infrastructure.Dao.Dao.Track;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class AlbumDaoConfiguration : IEntityTypeConfiguration<AlbumDao>
{
    public void Configure(EntityTypeBuilder<AlbumDao> builder)
    {
        builder.ToTable("Albums");

        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id).ValueGeneratedNever();

        builder.Property(album => album.Name);

        builder.Property(album => album.ReleaseDate);

        builder
            .HasMany<TrackDao>()
            .WithOne()
            .HasForeignKey(track => track.AlbumId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}