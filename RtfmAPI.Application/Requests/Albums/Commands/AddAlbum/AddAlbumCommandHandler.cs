using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Albums.Commands.AddAlbum.Dtos;
using RtfmAPI.Domain.Models.Albums;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;

/// <summary>
/// Обработчик команды добавления музыкального альбома.
/// </summary>
public class AddAlbumCommandHandler : IRequestHandler<AddAlbumCommand, Result<AddedAlbum>>
{
    private readonly IAlbumsCommandsRepository _repository;

    /// <summary>
    /// Обработчик команды добавления музыкального альбома.
    /// </summary>
    /// <param name="repository">Репозиторий музыкальных альбомов.</param>
    public AddAlbumCommandHandler(IAlbumsCommandsRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Обработка команды добавления музыкального альбома.
    /// </summary>
    /// <param name="request">Команда добавления музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    public async Task<Result<AddedAlbum>> Handle(AddAlbumCommand request, CancellationToken cancellationToken = default)
    {
        var albumsFabric = new AlbumsFabric(request.Name ?? string.Empty, request.ReleaseDate);
        var createAlbumResult = albumsFabric.Create();
        if (createAlbumResult.IsFailed)
        {
            return createAlbumResult.Error;
        }

        var album = createAlbumResult.Value;

        await _repository.CommitChangesAsync(album, cancellationToken);
        return new AddedAlbum
        {
            Id = album.Id.Value,
            Name = album.Name.Value
        };
    }
}