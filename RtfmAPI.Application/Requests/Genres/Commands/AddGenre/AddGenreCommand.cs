using FluentResults;
using MediatR;
using RtfmAPI.Application.Requests.Genres.Commands.AddGenre.Dtos;

namespace RtfmAPI.Application.Requests.Genres.Commands.AddGenre;

/// <summary>
/// Команда добавления музыкального жанра.
/// </summary>
public class AddGenreCommand : IRequest<Result<AddedGenre>>
{
    /// <summary>
    /// Создание команды добавления музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкальной группы.</param>
    public AddGenreCommand(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Название группы.
    /// </summary>
    public string Name { get; init; }
}