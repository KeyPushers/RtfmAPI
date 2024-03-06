using System;
using System.Collections.Generic;
using System.Linq;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands;

/// <summary>
/// Фабрика музыкальных групп.
/// </summary>
public class BandsFabric
{
    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Band> Restore(Guid id, string name, IEnumerable<Guid> albumIds)
    {
        var bandId = BandId.Create(id);
        
        var getBandNameResult = BandName.Create(name);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.Error;
        }
        
        var bandName = getBandNameResult.Value;

        var bandAlbumIds = albumIds.Select(AlbumId.Create).ToList();

        return Band.Restore(bandId, bandName, bandAlbumIds);
    }
    
    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="id">Идентификатор музыкальной группы.</param>
    /// <param name="name">Название музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    public Result<Band> Restore(Guid id, string name)
    {
        return Restore(id, name, Enumerable.Empty<Guid>());
    }
}