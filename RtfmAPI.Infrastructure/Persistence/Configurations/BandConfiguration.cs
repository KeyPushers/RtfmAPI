using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Aggregates.Bands;
using RftmAPI.Domain.Aggregates.Bands.ValueObjects;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class BandConfiguration : IEntityTypeConfiguration<Band>
{
    public void Configure(EntityTypeBuilder<Band> builder)
    {
        // Id
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasConversion(entity => entity.Value, id => BandId.Create(id));

        // Name
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .HasConversion(entity => entity.Name,
                name => new BandName(name));
    }
}