using MediatR;
using RftmAPI.Domain.Models.Bands;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand;

/// <summary>
/// Команда добавления музыкальной группы.
/// </summary>
public class AddBandCommand : IRequest<Band>
{
    /// <summary>
    /// Название группы.
    /// </summary>
    public string? Name { get; init; }
}