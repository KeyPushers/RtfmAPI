using System;

namespace RtfmAPI.Domain.Exceptions;

/// <summary>
/// Исключение доменных моделей.
/// </summary>
internal abstract class DomainException : Exception
{
    /// <summary>
    /// Создание исключения доменной модели.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    protected DomainException(string message) : base($"{nameof(DomainException)}.{message}")
    {
    }
}