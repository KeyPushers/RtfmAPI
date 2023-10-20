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
    public static class TrackFileData
    {
        /// <summary>
        /// Ошибка незаданного расширения файла музыкального трека.
        /// </summary>
        public static readonly Error IsEmpty = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileData)}.{nameof(IsEmpty)}",
            "Файл пуст.");
    }
}