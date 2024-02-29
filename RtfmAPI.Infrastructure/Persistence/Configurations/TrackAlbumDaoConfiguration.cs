// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using RtfmAPI.Infrastructure.Dao.Dao.Genre;
// using RtfmAPI.Infrastructure.Dao.Dao.TrackAlbum;
// using RtfmAPI.Infrastructure.Dao.Dao.TrackGenre;
//
// namespace RtfmAPI.Infrastructure.Persistence.Configurations;
//
// public class TrackAlbumDaoConfiguration : IEntityTypeConfiguration<TrackAlbumDao>
// {
//     public void Configure(EntityTypeBuilder<TrackAlbumDao> builder)
//     {
//         builder.ToTable("TrackAlbum");
//
//         builder.HasKey(trackAlbum => new {trackAlbum.TrackId, trackAlbum.AlbumId});
//
//         builder
//             .Property(trackAlbum => trackAlbum.TrackId)
//             .ValueGeneratedNever();
//         
//         builder
//             .Property(trackAlbum => trackAlbum.AlbumId)
//             .ValueGeneratedNever();
//
//         builder
//             .HasOne(trackAlbum => trackAlbum.Track)
//             .WithMany(track => track.GenreIds)
//             .HasForeignKey(trackAlbum => trackAlbum.TrackId);
//         
//         builder
//             .HasOne(trackAlbum => trackAlbum.Genre)
//             .WithMany(track => track.TrackIds)
//             .HasForeignKey(trackAlbum => trackAlbum.GenreId);
//     }
//     
// }