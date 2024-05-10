using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Fabrics.Albums;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Application.Requests.Albums.Commands.AddAlbum.Dtos;

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
        var albumsFactory = new AlbumsFactory(new ()
        {
            Name = request.Name,
            ReleaseDate = request.ReleaseDate
        });
        
        var createAlbumResult = albumsFactory.Create();
        if (createAlbumResult.IsFailed)
        {
            return createAlbumResult.ToResult();
        }

        var album = createAlbumResult.ValueOrDefault;

        await _repository.CommitChangesAsync(album, cancellationToken);
        
        return new AddedAlbum
        {
            Id = album.Id.Value,
            Name = album.Name.Value
        };
    }
}