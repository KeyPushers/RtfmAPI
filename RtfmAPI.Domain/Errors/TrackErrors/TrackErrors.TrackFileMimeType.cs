using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.TrackErrors;

/// <summary>
/// Ошибки доменной модели музыкального трека.
/// </summary>
public static partial class TrackErrors
{
    /// <summary>
    /// Ошибки MIME-типа файла музыкального трека.
    /// </summary>
    public static class TrackFileMimeType
    {
        /// <summary>
        /// Ошибка неуказанного MIME-типа.
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileMimeType)}.{nameof(IsNullOrWhiteSpace)}", "MIME-тип не указан.");
        
        /// <summary>
        /// Ошибка неуказанного MIME-типа.
        /// </summary>
        public static readonly Error Invalid = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileMimeType)}.{nameof(Invalid)}", "Некорректный MIME-тип.");
    }
}