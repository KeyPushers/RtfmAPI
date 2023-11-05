﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RftmAPI.Domain.Models.Albums;
using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Primitives;
using RtfmAPI.Application.Common.Interfaces.Persistence;

namespace RtfmAPI.Application.Requests.Albums.Commands.AddAlbum;

/// <summary>
/// Обработчик команды добавления музыкального альбома.
/// </summary>
public class AddAlbumCommandHandler : IRequestHandler<AddAlbumCommand, Result<Album>>
{
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Обработчик команды добавления музыкального альбома.
    /// </summary>
    /// <param name="albumsRepository">Репозиторий альбомов.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    public AddAlbumCommandHandler(IAlbumsRepository albumsRepository, IUnitOfWork unitOfWork)
    {
        _albumsRepository = albumsRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Обработка команды добавления музыкального альбома.
    /// </summary>
    /// <param name="request">Команда добавления музыкального альбома.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Музыкальный трек.</returns>
    public async Task<Result<Album>> Handle(AddAlbumCommand request, CancellationToken cancellationToken = default)
    {
        var albumNameResult = AlbumName.Create(request.Name ?? string.Empty);
        if (albumNameResult.IsFailed)
        {
            return albumNameResult.Error;
        }

        var albumReleaseDateResult = AlbumReleaseDate.Create(request.ReleaseDate);
        if (albumReleaseDateResult.IsFailed)
        {
            return albumReleaseDateResult.Error;
        }

        var albumResult = Album.Create(albumNameResult.Value, albumReleaseDateResult.Value);
        if (albumResult.IsFailed)
        {
            return albumResult.Error;
        }

        await _albumsRepository.AddAsync(albumResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return albumResult;
    }
}