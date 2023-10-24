namespace RftmAPI.Domain.Utils;

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