using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Albums.Commands.DeleteAlbumById;

/// <summary>
/// Обработчик команды удаления альбома.
/// </summary>
public class DeleteAlbumByIdCommandHandler : IRequestHandler<DeleteAlbumByIdCommand, Result<Unit>>
{
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Создание обработчика команды удаления альбома.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий альбомов.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public DeleteAlbumByIdCommandHandler(IAlbumsRepository albumsRepository, IUnitOfWork unitOfWork)
    {
        _albumsRepository = albumsRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды удаления альбома.
    /// </summary>
    /// <param name="request">Команда удаления альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<Result<Unit>> Handle(DeleteAlbumByIdCommand request, CancellationToken cancellationToken = default)
    {
        var albumId = AlbumId.Create(request.Id);
        
        var album = await _albumsRepository.GetAlbumByIdAsync(albumId);
        if (album is null)
        {
            return AlbumExceptions.NotFound(albumId);
        }

        var deleteAlbumResult = album.Delete();
        if (deleteAlbumResult.IsFailed)
        {
            return deleteAlbumResult.Error;
        }
        
        await _albumsRepository.DeleteAlbumAsync(album);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}