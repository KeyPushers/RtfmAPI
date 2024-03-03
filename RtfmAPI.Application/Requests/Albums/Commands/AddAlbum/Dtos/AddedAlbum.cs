using System;

namespace RtfmAPI.Application.Requests.Albums.Commands.AddAlbum.Dtos;

/// <summary>
/// Объект переноса данных добавленного музыкального альбома.
/// </summary>
public class AddedAlbum
{
    /// <summary>
    /// Идентификатор добавленного музыкального альбома.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название добавленного музыкального альбома.
    /// </summary>
    public string? Name { get; init; }
}