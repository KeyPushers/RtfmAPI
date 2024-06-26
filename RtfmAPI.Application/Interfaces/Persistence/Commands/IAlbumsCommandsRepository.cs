﻿using RtfmAPI.Application.Interfaces.Persistence.Common;
using RtfmAPI.Domain.Models.Albums;

namespace RtfmAPI.Application.Interfaces.Persistence.Commands;

/// <summary>
/// Интерфейс репозитория комманд доменной модели <see cref="Album"/>.
/// </summary>
public interface IAlbumsCommandsRepository : ICommit<Album>
{
}