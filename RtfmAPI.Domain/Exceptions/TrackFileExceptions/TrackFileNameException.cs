﻿using RftmAPI.Domain.Exceptions.TrackExceptions;

namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключение названия файла доменной модели музыкального трека.
/// </summary>
public sealed class TrackFileNameException : TrackException
{
    /// <summary>
    /// Создание исключения доменной модели музыкального трека.
    /// </summary>
    /// <param name="message">Сообщение</param>
    public TrackFileNameException(string message) : base($"{nameof(TrackFileNameException)}. {message}")
    {
    }
}