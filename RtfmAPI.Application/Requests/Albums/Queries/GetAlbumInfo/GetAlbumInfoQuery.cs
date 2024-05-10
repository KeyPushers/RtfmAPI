﻿using System;
using FluentResults;
using MediatR;
using RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo.Dtos;

namespace RtfmAPI.Application.Requests.Albums.Queries.GetAlbumInfo;

/// <summary>
/// Запрос информации о музыкальном альбома.
/// </summary>
public class GetAlbumInfoQuery : IRequest<Result<AlbumInfo>>
{
    /// <summary>
    /// Идентификатор музыкального альбома.
    /// </summary>
    public Guid Id { get; init; }
}