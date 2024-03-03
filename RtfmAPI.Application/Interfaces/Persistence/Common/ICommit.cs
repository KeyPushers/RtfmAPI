using System.Threading.Tasks;

namespace RtfmAPI.Application.Interfaces.Persistence.Common;

public interface ICommit<in TDomainModel>
{
    /// <summary>
    /// Сохранение изменений доменной модели.
    /// </summary>
    /// <param name="value">Тип доменной модели.</param>
    Task CommitChangesAsync(TDomainModel value);
}