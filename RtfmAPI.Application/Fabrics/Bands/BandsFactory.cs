using System.Linq;
using FluentResults;
using RtfmAPI.Application.Fabrics.Bands.Daos;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Models.Genres.ValueObjects;

namespace RtfmAPI.Application.Fabrics.Bands;

/// <summary>
/// Фабрика музыкальных групп.
/// </summary>
public class BandsFactory : IEntityFactory<Band, BandId>
{
    private readonly BandDao _bandDao;

    /// <summary>
    /// Создание фабрики музыкальных групп.
    /// </summary>
    /// <param name="bandDao">Объект доступа данных музыкальной группы.</param>
    public BandsFactory(BandDao bandDao)
    {
        _bandDao = bandDao;
    }

    /// <inheritdoc/>
    public Result<Band> Create()
    {
        var getBandNameResult = BandName.Create(_bandDao.Name ?? string.Empty);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.ToResult();
        }

        var bandName = getBandNameResult.ValueOrDefault;

        return Band.Create(bandName);
    }

    /// <inheritdoc/>
    public Result<Band> Restore()
    {
        var bandId = BandId.Create(_bandDao.Id);

        var getBandNameResult = BandName.Create(_bandDao.Name ?? string.Empty);
        if (getBandNameResult.IsFailed)
        {
            return getBandNameResult.ToResult();
        }

        var bandName = getBandNameResult.ValueOrDefault;

        var bandAlbumIds = _bandDao.AlbumIds.Select(AlbumId.Create);
        var bandGenreIds = _bandDao.GenreIds.Select(GenreId.Create);

        return Band.Restore(bandId, bandName, bandAlbumIds, bandGenreIds);
    }
}