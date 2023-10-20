using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.TrackErrors;

/// <summary>
/// Ошибки доменной модели музыкального трека.
/// </summary>
public static partial class TrackErrors
{
    /// <summary>
    /// Ошибки названия музыкальной трека.
    /// </summary>
    public static class TrackName
    {
        /// <summary>
        /// Ошибка незаданного наименования музыкального альбома.
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(TrackErrors)}.{nameof(TrackName)}.{nameof(IsNullOrWhiteSpace)}",
            "Название музыкального трека не задано.");

        /// <summary>
        /// Ошибка короткого наименования музыкального альбома.
        /// </summary>
        public static readonly Error IsTooShort = new(
            $"{nameof(TrackErrors)}.{nameof(TrackName)}.{nameof(IsTooShort)}",
            "Название музыкального трека слишком короткое.");

        /// <summary>
        /// Ошибка длинного наименования альбома.
        /// </summary>
        public static readonly Error IsTooLong = new(
            $"{nameof(TrackErrors)}.{nameof(TrackName)}.{nameof(IsTooLong)}",
            "Название музыкального трека слишком длинное.");
    }
}