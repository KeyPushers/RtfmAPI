using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Fabrics;

/// <summary>
/// Фабрик музыкальных групп.
/// </summary>
public class BandsFabric
{
    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Band> CreateBand(string name, IEnumerable<Guid> albumIds)
    {
        var getBandNameResult = BandName.Create(name);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.Error;
        }

        var bandName = getBandNameResult.Value;

        var bandAlbumIds = albumIds.Select(AlbumId.Create).ToList();

        return Band.Create(bandName, bandAlbumIds);
    }
    
    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Band> CreateBand(string name)
    {
        var getBandNameResult = BandName.Create(name);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.Error;
        }

        var bandName = getBandNameResult.Value;
        
        return Band.Create(bandName);
    }
}