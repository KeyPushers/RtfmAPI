﻿using RtfmAPI.Application.Interfaces.Persistence.Common;
using RtfmAPI.Domain.Models.Genres;

namespace RtfmAPI.Application.Interfaces.Persistence.Commands;

/// <summary>
/// Интерфейс репозитория комманд доменной модели <see cref="Genre"/>.
/// </summary>
public interface IGenresCommandsRepository : ICommit<Genre>
{
}