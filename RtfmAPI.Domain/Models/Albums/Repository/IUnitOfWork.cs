namespace RftmAPI.Domain.Models.Albums.Repository;

/// <summary>
/// Интерфейс единицы работы.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохранение изменения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}