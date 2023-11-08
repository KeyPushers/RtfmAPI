using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий музыкальных жанров.
/// </summary>
public class GenresRepository : IGenresRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Создание репозитория музыкальных жанров.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public GenresRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    /// </summary>
    public Task<List<Genre>> GetGenresAsync()
    {
        return _context.Set<Genre>().ToListAsync();
    }

    /// <summary>
    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    /// </summary>
    public Task<Genre?> GetGenreByIdAsync(GenreId genreId)
    {
        return _context.Set<Genre>().FirstOrDefaultAsync(entity => entity.Id == GenreId.Create(genreId.Value));
    }

    /// <summary>
    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    /// </summary>
    public async Task AddAsync(Genre genre)
    {
        await _context.AddAsync(genre);
    }
}