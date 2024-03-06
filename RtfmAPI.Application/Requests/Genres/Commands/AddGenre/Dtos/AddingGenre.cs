using System.ComponentModel.DataAnnotations;

namespace RtfmAPI.Application.Requests.Genres.Commands.AddGenre.Dtos;

/// <summary>
/// Объект переноса данных добавляемого музыкального жанра.
/// </summary>
public sealed class AddingGenre
{
    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    [Required]
    public string? Name { get; init; }
}