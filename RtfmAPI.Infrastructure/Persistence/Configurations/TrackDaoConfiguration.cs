using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RftmAPI.Domain.Models.Tracks.ValueObjects;
using RtfmAPI.Infrastructure.Dao.Dao.TrackFiles;
using RtfmAPI.Infrastructure.Dao.Dao.Tracks;

namespace RtfmAPI.Infrastructure.Persistence.Configurations;

public class TrackDaoConfiguration : IEntityTypeConfiguration<TrackDao>
{
    public void Configure(EntityTypeBuilder<TrackDao> builder)
    {
        // Определение названия таблицы музыкальных треков.
        builder.ToTable("Tracks");

        // Определение идентификатора музыкального трека.
        builder.HasKey(track => track.Id);
        builder.Property(track => track.Id)
            .ValueGeneratedNever();

        // Определение названия музыкального трека.
        builder
            .Property(track => track.Name)
            .HasMaxLength(TrackName.MaxLength);

        // Определение даты выпуска музыкального трека.
        builder.Property(track => track.ReleaseDate);
        
        // Определение идентификатора файла музыкального трека.
        builder
            .HasOne<TrackFileDao>()
            .WithOne()
            .HasForeignKey<TrackDao>(entity => entity.TrackFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}