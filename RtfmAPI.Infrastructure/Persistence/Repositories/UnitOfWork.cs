using RftmAPI.Domain.Utils;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Представление единицы работы.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Создание экземпляра единицы работы.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc cref="IUnitOfWork.SaveChangesAsync"/>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}