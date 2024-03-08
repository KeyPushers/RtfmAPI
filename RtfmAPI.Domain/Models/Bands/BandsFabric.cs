using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Fabrics;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands;

/// <summary>
/// Фабрика музыкальных групп.
/// </summary>
public class BandsFabric : AggregateFabric<Band, BandId, Guid>
{
    private readonly string _name;
    private readonly IEnumerable<Guid> _albumIds;
    private readonly IEnumerable<Guid> _genreIds;

    /// <summary>
    /// Создание фабрики музыкальных групп.
    /// </summary>
    /// <param name="name">название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    /// <param name="genreIds">Идентификаторы музыкальных жанров.</param>
    public BandsFabric(string name, IEnumerable<Guid> albumIds, IEnumerable<Guid> genreIds)
    {
        _name = name;
        _albumIds = albumIds;
        _genreIds = genreIds;
    }
    
    /// <inheritdoc />
    public override Result<Band> Create()
    {
        var getBandNameResult = BandName.Create(_name);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.Error;
        }
        
        var bandName = getBandNameResult.Value;
        
        return Band.Create(bandName);
    }

    /// <inheritdoc />
    public override Result<Band> Restore(Guid id)
    {
        var bandId = BandId.Create(id);
        
        var getBandNameResult = BandName.Create(_name);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.Error;
        }
        
        var bandName = getBandNameResult.Value;

        var bandAlbumIds = _albumIds.Select(AlbumId.Create);
        var bandGenreIds = _genreIds.Select(GenreId.Create);
        
        return Band.Restore(bandId, bandName, bandAlbumIds, bandGenreIds);
    }
}