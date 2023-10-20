using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.AlbumErrors;

/// <summary>
/// Ошибки доменной модели музыкального альбома.
/// </summary>
public static partial class AlbumErrors
{
    /// <summary>
    /// Ошибки названия музыкального альбома.
    /// </summary>
    public static class AlbumName
    {
        /// <summary>
        /// Ошибка незаданного наименования музыкальног
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(AlbumErrors)}.{nameof(AlbumName)}.{nameof(IsNullOrWhiteSpace)}",
            "Название музыкального альбома не задано.");

        /// <summary>
        /// Ошибка короткого наименования музыкального альбома.
        /// </summary>
        public static readonly Error IsTooShort = new(
            $"{nameof(AlbumErrors)}.{nameof(AlbumName)}.{nameof(IsTooShort)}",
            "Название музыкального альбома слишком короткое.");

        /// <summary>
        /// Ошибка длинного наименования альбома.
        /// </summary>
        public static readonly Error IsTooLong = new(
            $"{nameof(AlbumErrors)}.{nameof(AlbumName)}.{nameof(IsTooLong)}",
            "Название музыкального альбома слишком длинное.");
    }
}