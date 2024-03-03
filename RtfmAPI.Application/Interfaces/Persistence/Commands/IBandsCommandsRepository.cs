using RtfmAPI.Application.Interfaces.Persistence.Common;
using RtfmAPI.Domain.Models.Bands;

namespace RtfmAPI.Application.Interfaces.Persistence.Commands;

/// <summary>
/// Интерфейс репозитория комманд доменной модели <see cref="Band"/>.
/// </summary>
public interface IBandsCommandsRepository : ICommit<Band>
{
}