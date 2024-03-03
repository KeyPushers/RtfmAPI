using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Models.Bands.Events;
using RtfmAPI.Domain.Models.Bands.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Bands;

/// <summary>
/// Музыкальная группа.
/// </summary>
public sealed class Band : AggregateRoot<BandId, Guid>
{
    private readonly HashSet<AlbumId> _albumIds = new();

    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public BandName Name { get; private set; }

    /// <summary>
    /// Музыкальные альбомы.
    /// </summary>
    public IReadOnlyCollection<AlbumId> AlbumIds => _albumIds;
    
    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    private Band(BandName name, IReadOnlyCollection<AlbumId> albumIds) : base(
        BandId.Create())
    {
        AddDomainEvent(new BandCreatedDomainEvent(this));

        Name = name;
        AddDomainEvent(new BandNameChangedDomainEvent(this, name));

        _albumIds = albumIds.ToHashSet();
        AddDomainEvent(new AlbumsAddedToBandDomainEvent(this, albumIds));
    }
    
    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <returns>Музыкальная группа.</returns>
    public static Result<Band> Create(BandName name)
    {
        return new Band(name, new List<AlbumId>());
    }

    /// <summary>
    /// Создание музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    /// <param name="albumIds">Идентификаторы музыкальных альбомов.</param>
    /// <returns>Музыкальная группа.</returns>
    public static Result<Band> Create(BandName name, IReadOnlyCollection<AlbumId> albumIds)
    {
        return new Band(name, albumIds);
    }

    /// <summary>
    /// Установление названия музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public BaseResult SetName(BandName name)
    {
        if (Name == name)
        {
            return BaseResult.Success();
        }

        Name = name;
        AddDomainEvent(new BandNameChangedDomainEvent(this, name));
        return BaseResult.Success();
    }

    /// <summary>
    /// Добавление музыкальных альбомов.
    /// </summary>
    /// <param name="albumIds">Идентификаторы добавляемых альбомов.</param>
    public BaseResult AddAlbums(IReadOnlyCollection<AlbumId> albumIds)
    {
        if (!albumIds.Any())
        {
            return BaseResult.Success();
        }

        List<AlbumId> addedAlbums = new();
        foreach (var albumId in albumIds)
        {
            if (_albumIds.Contains(albumId))
            {
                continue;
            }

            addedAlbums.Add(albumId);
            _albumIds.Add(albumId);
        }

        AddDomainEvent(new AlbumsAddedToBandDomainEvent(this, addedAlbums));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкальных альбомов.
    /// </summary>
    /// <param name="albums">Удаляемые альбомы.</param>
    public BaseResult RemoveAlbums(IReadOnlyCollection<Album> albums)
    {
        if (!albums.Any())
        {
            return BaseResult.Success();
        }

        List<Album> removedAlbums = new();
        foreach (var album in albums)
        {
            var albumId = AlbumId.Create(album.Id.Value);
            if (!_albumIds.Contains(albumId))
            {
                continue;
            }

            removedAlbums.Add(album);
            _albumIds.Remove(albumId);
        }

        AddDomainEvent(new AlbumsRemovedFromBandDomainEvent(this, removedAlbums));
        return BaseResult.Success();
    }
    
    /// <summary>
    /// Удаление музыкальной группы.
    /// </summary>
    /// <param name="deleteAction">Делегат, отвечающий за удаление музыкальной группы.</param>
    public async Task<BaseResult> DeleteAsync(Func<Band, Task<bool>> deleteAction)
    {
        var deleteActionResult = await deleteAction(this);
        if (!deleteActionResult)
        {
            return new ArgumentOutOfRangeException();
        }
        AddDomainEvent(new BandDeletedDomainEvent(this));
        return BaseResult.Success();
    }
}