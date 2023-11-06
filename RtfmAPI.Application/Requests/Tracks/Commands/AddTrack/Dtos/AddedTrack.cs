using System;

namespace RtfmAPI.Application.Requests.Tracks.Commands.AddTrack.Dtos;

/// <summary>
/// Объект переноса данных добавленного музыкального трека.
/// </summary>
public class AddedTrack
{
    /// <summary>
    /// Идентификатор музыкального трека.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Наименование музыкального трека.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Продолжительность музыкального трека.
    /// </summary>
    public double Duration { get; init; }

    /// <summary>
    /// Идентификатор файла.
    /// </summary>
    public Guid FileId { get; init; }

    /// <summary>
    /// Дата выпуска музыкального трека.
    /// </summary>
    public DateTime ReleaseDate { get; init; }
}