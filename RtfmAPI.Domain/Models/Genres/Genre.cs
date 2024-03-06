﻿using System;
using RtfmAPI.Domain.Models.Genres.Events;
using RtfmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Domain.Models.Genres;

/// <summary>
/// Музыкальный жанр.
/// </summary>
public sealed class Genre : AggregateRoot<GenreId, Guid>
{
    /// <summary>
    /// Название музыкального жанра.
    /// </summary>
    public GenreName Name { get; private set; }

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
#pragma warning disable CS8618
    private Genre()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="name">Название музыкального жанра.</param>
    private Genre(GenreId id, GenreName name) : base(id)
    {
        Name = name;
    }
    
    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    private Genre(GenreName name) : base(GenreId.Create())
    {
        Name = name;
    }

    /// <summary>
    /// Восстановление музыкального жанра.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="name">Название музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    internal static Genre Restore(GenreId id, GenreName name)
    {
        return new Genre(id, name);
    }
    
    /// <summary>
    /// Создание музыкального жанра.
    /// </summary>
    /// <param name="name">Название музыкального жанра.</param>
    /// <returns>Музыкальный жанр.</returns>
    public static Result<Genre> Create(GenreName name)
    {
        var genre = new Genre(name);
        genre.AddDomainEvent(new GenreNameChangedDomainEvent(genre, name));
        return genre;
    }
}