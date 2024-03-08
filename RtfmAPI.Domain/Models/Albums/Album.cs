﻿using System;
using System.Threading.Tasks;
using RtfmAPI.Domain.Models.Albums.Events;
using RtfmAPI.Domain.Models.Albums.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Albums;

/// <summary>
/// Музыкальный альбом.
/// </summary>
public sealed class Album : AggregateRoot<AlbumId, Guid>
{
    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public AlbumName Name { get; private set; }

    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    public AlbumReleaseDate ReleaseDate { get; private set; }

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    private Album(AlbumName name, AlbumReleaseDate releaseDate) : this(AlbumId.Create(), name, releaseDate)
    {
    }

    /// <summary>
    /// Музыкальный альбом.
    /// </summary>
    /// <param name="id">Идентификатор музыкального альбома.</param>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    private Album(AlbumId id, AlbumName name, AlbumReleaseDate releaseDate) : base(id)
    {
        Name = name;
        ReleaseDate = releaseDate;
    }

    /// <summary>
    /// Создание музыкального альбома.
    /// </summary>
    /// <param name="name">Название музыкального альбома.</param>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    /// <returns>Музыкальный альбом.</returns>
    internal static Result<Album> Create(AlbumName name, AlbumReleaseDate releaseDate)
    {
        var album = new Album(name, releaseDate);
        album.AddDomainEvent(new AlbumCreatedDomainEvent(album));
        album.AddDomainEvent(new AlbumNameChangedDomainEvent(album, name));
        album.AddDomainEvent(new AlbumReleaseDateChangedDomainEvent(album, releaseDate));

        return album;
    }

    /// <summary>
    /// Восстановление музыкального альбома.
    /// </summary>
    internal static Result<Album> Restore(AlbumId id, AlbumName name, AlbumReleaseDate releaseDate)
    {
        return new Album(id, name, releaseDate);
    }

    /// <summary>
    /// Изменение названия музыкального альбома.
    /// </summary>
    /// <param name="name">Название альбома.</param>
    public BaseResult SetName(AlbumName name)
    {
        if (Name == name)
        {
            return BaseResult.Success();
        }

        Name = name;
        AddDomainEvent(new AlbumNameChangedDomainEvent(this, name));
        return BaseResult.Success();
    }

    /// <summary>
    /// Изменение даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="releaseDate">Дата выпуска музыкального альбома.</param>
    public BaseResult SetReleaseDate(AlbumReleaseDate releaseDate)
    {
        if (ReleaseDate == releaseDate)
        {
            return BaseResult.Success();
        }

        ReleaseDate = releaseDate;
        AddDomainEvent(new AlbumReleaseDateChangedDomainEvent(this, releaseDate));
        return BaseResult.Success();
    }

    /// <summary>
    /// Удаление музыкального альбома.
    /// </summary>
    /// <param name="deleteAction">Делегат, отвечающий за удаление музыкального альбома.</param>
    public async Task<BaseResult> DeleteAsync(Func<Album, Task<bool>> deleteAction)
    {
        var deleteActionResult = await deleteAction(this);
        if (!deleteActionResult)
        {
            return new InvalidOperationException();
        }

        AddDomainEvent(new AlbumDeletedDomainEvent(this));
        return BaseResult.Success();
    }
}