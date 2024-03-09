using System.Threading;
using System.Threading.Tasks;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Application.Interfaces.Persistence.Common;

public interface ICommit<in TDomainModel> where TDomainModel : IHasDomainEvents
{
    /// <summary>
    /// Сохранение изменений доменной модели.
    /// </summary>
    /// <param name="value">Тип доменной модели.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task CommitChangesAsync(TDomainModel value, CancellationToken cancellationToken = default);
}