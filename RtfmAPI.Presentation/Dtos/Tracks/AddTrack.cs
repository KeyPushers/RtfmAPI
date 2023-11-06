using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RtfmAPI.Presentation.Dtos.Tracks;

/// <summary>
/// Объект переноса команды добавления музыкального трека.
/// </summary>
public sealed class AddTrack
{
    /// <summary>
    /// Название музыкального трека.
    /// </summary>
    [Required]
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    [Required]
    public DateTime ReleaseDate { get; init; }

    /// <summary>
    /// Музыкальный файл.
    /// </summary>
    [Required]
    public IFormFile? File { get; init; }
}