using System;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;

/// <summary>
/// Объект переноса данных добавленной музыкальной группы.
/// </summary>
public sealed class AddedBand
{
    /// <summary>
    /// Идентификатор музыкальной группы.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    public string? Name { get; init; }
}