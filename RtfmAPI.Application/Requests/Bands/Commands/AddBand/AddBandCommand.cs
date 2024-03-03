using MediatR;
using RtfmAPI.Application.Requests.Bands.Commands.AddBand.Dtos;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Bands.Commands.AddBand;

/// <summary>
/// Команда добавления музыкальной группы.
/// </summary>
public class AddBandCommand : IRequest<Result<AddedBand>>
{
    /// <summary>
    /// Создание команды добавления музыкальной группы.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public AddBandCommand(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Название группы.
    /// </summary>
    public string Name { get; init; }
}