using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.Albums;
using RtfmAPI.Infrastructure.Dao.Dao.Bands;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class BandsConfiguration : IEntityTypeConfiguration<BandDao>
{
    public void Configure(EntityTypeBuilder<BandDao> builder)
    {
        builder.ToTable("Bands");

        builder.HasKey(band => band.Id);
        builder.Property(band => band.Id).ValueGeneratedNever();

        builder.Property(band => band.Name);

        builder.HasMany<AlbumDao>();
    }
}