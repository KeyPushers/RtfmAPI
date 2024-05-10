using RtfmAPI.Domain.Models.Tracks;

namespace RtfmAPI.Application.Interfaces.Persistence.Commands;

/// <summary>
/// Интерфейс репозитория комманд доменной модели <see cref="Track"/>.
/// </summary>
public interface ITracksCommandsRepository : IUnitOfWork<Track>
{
}