using System;
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
    public string? Name { get; init; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public DateTime ReleaseDate { get; init; }

    /// <summary>
    /// Музыкальный файл.
    /// </summary>
    public IFormFile? File { get; init; }
}