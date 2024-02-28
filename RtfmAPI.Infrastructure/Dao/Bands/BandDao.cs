namespace RtfmAPI.Infrastructure.Dao.Bands;

public class BandDao
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<Guid> AlbumIds { get; set; } = new();
    public List<Guid> GenreIds { get; set; } = new();
}