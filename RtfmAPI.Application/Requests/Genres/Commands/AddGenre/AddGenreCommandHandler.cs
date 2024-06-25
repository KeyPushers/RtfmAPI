using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Genres.Commands.AddGenre.Dtos;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Genres.Commands.AddGenre;

/// <summary>
/// Обработчик команды добавления музыкального жанра.
/// </summary>
public class AddGenreCommandHandler : IRequestHandler<AddGenreCommand, Result<AddedGenre>>
{
    private readonly IGenresCommandsRepository _repository;

    /// <summary>
    /// Создание обработчика команды добавления музыкального жанра.
    /// </summary>
    /// <param name="repository">Репозиторий команд музыкального жанра.</param>
    public AddGenreCommandHandler(IGenresCommandsRepository repository)
    {
        _repository = repository;
    }
    
    /// <summary>
    /// Обработка команды добавления музыкального жанра.
    /// </summary>
    /// <param name="request">Команда добавления музыкального жанра.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект переноса данных добавленного жанра.</returns>
    public async Task<Result<AddedGenre>> Handle(AddGenreCommand request, CancellationToken cancellationToken = default)
    {
        // TODO: Добавить проверку существования музыкального жанра с тем же именем.
        
        var genresFabric = new GenresFabric(request.Name);

        var createGenreResult = genresFabric.Create();
        if (createGenreResult.IsFailed)
        {
            return createGenreResult.Error;
        }

        var genre = createGenreResult.Value;
        
        await _repository.CommitChangesAsync(genre, cancellationToken);

        return new AddedGenre
        {
            Id = genre.Id.Value,
            Name = genre.Name.Value
        };
    }
}