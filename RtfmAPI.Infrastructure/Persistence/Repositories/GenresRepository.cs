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
    private readonly DbSet<Genre> _context;

    /// <summary>
    /// Создание репозитория музыкальных жанров.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public GenresRepository(AppDbContext context)
    {
        _context = context.Set<Genre>();
    }

    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    public Task<List<Genre>> GetGenresAsync()
    {
        return _context.ToListAsync();
    }

    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    public Task<Genre?> GetGenreByIdAsync(GenreId genreId)
    {
        return _context.FirstOrDefaultAsync(entity => ReferenceEquals(entity.Id, genreId));
    }

    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    public async Task AddAsync(Genre genre)
    {
        await _context.AddAsync(genre);
    }

    /// <inheritdoc cref="IGenresRepository.UpdateAsync"/>
    public Task UpdateAsync(Genre genre)
    {
        _context.Update(genre);
        return Task.CompletedTask;
    }
}