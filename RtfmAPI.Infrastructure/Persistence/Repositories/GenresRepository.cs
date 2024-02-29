using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RftmAPI.Domain.Models.Genres;
using RftmAPI.Domain.Models.Genres.ValueObjects;
using RtfmAPI.Application.Common.Interfaces.Persistence;
using RtfmAPI.Infrastructure.Dao.Dao.Genre;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Репозиторий музыкальных жанров.
/// </summary>
public class GenresRepository : IGenresRepository
{
    private readonly IMapper _mapper;
    private readonly DbSet<GenreDao> _context;

    /// <summary>
    /// Создание репозитория музыкальных жанров.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="mapper">Маппер.</param>
    public GenresRepository(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context.Set<GenreDao>();
    }

    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    public Task<List<Genre>> GetGenresAsync()
    {
        return _context.Select(entity => _mapper.Map<Genre>(entity)).ToListAsync();
    }

    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    public async Task<Genre?> GetGenreByIdAsync(GenreId genreId)
    {
        var genreDao = await _context.FirstOrDefaultAsync(entity => entity.Id == genreId.Value);
        return genreDao is null ? null : _mapper.Map<Genre>(genreDao);
    }

    /// <inheritdoc cref="IGenresRepository.GetGenresAsync"/>
    public async Task AddAsync(Genre genre)
    {
        var genreDao = _mapper.Map<GenreDao>(genre);
        await _context.AddAsync(genreDao);
    }

    /// <inheritdoc cref="IGenresRepository.UpdateAsync"/>
    public Task UpdateAsync(Genre genre)
    {
        var genreDao = _mapper.Map<GenreDao>(genre);
        _context.Update(genreDao);
        return Task.CompletedTask;
    }
}