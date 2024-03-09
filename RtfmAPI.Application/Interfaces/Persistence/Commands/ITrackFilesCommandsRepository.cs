using RtfmAPI.Application.Interfaces.Persistence.Common;
using RtfmAPI.Domain.Models.TrackFiles;

namespace RtfmAPI.Application.Interfaces.Persistence.Commands;

/// <summary>
/// Интерфейс репозитория комманд доменной модели <see cref="TrackFile"/>.
/// </summary>
public interface ITrackFilesCommandsRepository : ICommit<TrackFile>
{
    
}