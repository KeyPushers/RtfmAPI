using System.Threading;
using System.Threading.Tasks;

namespace RtfmAPI.Application.Common.Interfaces.Persistence;

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