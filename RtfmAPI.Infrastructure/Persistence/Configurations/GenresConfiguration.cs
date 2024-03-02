using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class GenresConfiguration : IEntityTypeConfiguration<GenreDao>
{
    public void Configure(EntityTypeBuilder<GenreDao> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(genre => genre.Id);
        builder.Property(genre => genre.Id)
            .ValueGeneratedNever();

        builder.Property(genre => genre.Name);
    }
}