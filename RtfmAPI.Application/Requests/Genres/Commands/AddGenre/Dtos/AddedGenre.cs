using System;

namespace RtfmAPI.Application.Requests.Genres.Commands.AddGenre.Dtos;

/// <summary>
/// Объект переноса данных добавленного жанра.
/// </summary>
public sealed class AddedGenre
{
    /// <summary>
    /// Идентификатор музыкального жанра.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    public string? Name { get; init; }
}