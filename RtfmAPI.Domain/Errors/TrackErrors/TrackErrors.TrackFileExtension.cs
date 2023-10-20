using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.TrackErrors;

/// <summary>
/// Ошибки доменной модели музыкального трека.
/// </summary>
public static partial class TrackErrors
{
    /// <summary>
    /// Ошибки расширения файла музыкального трека.
    /// </summary>
    public static class TrackFileExtension
    {
        /// <summary>
        /// Ошибка незаданного расширения файла музыкального трека.
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileExtension)}.{nameof(IsNullOrWhiteSpace)}",
            "Расширение файла музыкального трека не задано.");
        
        /// <summary>
        /// Ошибка некорректного названия расширения музыкального трека.
        /// </summary>
        public static readonly Error Invalid = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileExtension)}.{nameof(Invalid)}",
            "Некорректное название расширения.");
    }
}