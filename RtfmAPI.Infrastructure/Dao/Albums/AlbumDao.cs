namespace RtfmAPI.Infrastructure.Dao.Albums;

public class AlbumDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Guid> TrackIds { get; set; } = new();
    public List<Guid> BandIds { get; set; } = new();
}