using System.ComponentModel.DataAnnotations;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;

/// <summary>
/// Объект переноса данных добавляемой музыкальной группы.
/// </summary>
public sealed class AddingBand
{
    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    [Required]
    public string? Name { get; init; }
}