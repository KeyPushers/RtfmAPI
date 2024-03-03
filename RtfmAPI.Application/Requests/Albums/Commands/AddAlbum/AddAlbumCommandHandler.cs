using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RtfmAPI.Application.Fabrics;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Albums.Commands.AddAlbum.Dtos;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;

/// <summary>
/// Обработчик команды добавления музыкального альбома.
/// </summary>
public class AddAlbumCommandHandler : IRequestHandler<AddAlbumCommand, Result<AddedAlbum>>
{
    private readonly IAlbumsCommandsRepository _repository;
    private readonly AlbumsFabric _albumsFabric;

    /// <summary>
    /// Обработчик команды добавления музыкального альбома.
    /// </summary>
    /// <param name="repository">Репозиторий музыкальных альбомов.</param>
    /// <param name="albumsFabric">Фабрика музыкальных альбомов.</param>
    public AddAlbumCommandHandler(IAlbumsCommandsRepository repository, AlbumsFabric albumsFabric)
    {
        _repository = repository;
        _albumsFabric = albumsFabric;
    }

    /// <summary>
    /// Обработка команды добавления музыкального альбома.
    /// </summary>
    /// <param name="request">Команда добавления музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    public async Task<Result<AddedAlbum>> Handle(AddAlbumCommand request, CancellationToken cancellationToken = default)
    {
        var createAlbumResult = _albumsFabric.CreateAlbum(request.Name ?? string.Empty, request.ReleaseDate);
        if (createAlbumResult.IsFailed)
        {
            return createAlbumResult.Error;
        }

        var album = createAlbumResult.Value;

        await _repository.CommitChangesAsync(album);

        return new AddedAlbum
        {
            Id = album.Id.Value,
            Name = album.Name.Value
        };
    }
}