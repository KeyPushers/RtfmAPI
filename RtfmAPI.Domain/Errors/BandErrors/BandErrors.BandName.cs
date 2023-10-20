using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.BandErrors;

/// <summary>
/// Ошибки доменной модели музыкальной группы.
/// </summary>
public static partial class BandErrors
{
    /// <summary>
    /// Ошибки названия музыкальной группы.
    /// </summary>
    public static class BandName
    {
        /// <summary>
        /// Ошибка незаданного наименования музыкальног
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(BandErrors)}.{nameof(BandName)}.{nameof(IsNullOrWhiteSpace)}",
            "Название музыкальной группы не задано.");

        /// <summary>
        /// Ошибка короткого наименования музыкального альбома.
        /// </summary>
        public static readonly Error IsTooShort = new(
            $"{nameof(BandErrors)}.{nameof(BandName)}.{nameof(IsTooShort)}",
            "Название музыкальной группы слишком короткое.");

        /// <summary>
        /// Ошибка длинного наименования альбома.
        /// </summary>
        public static readonly Error IsTooLong = new(
            $"{nameof(BandErrors)}.{nameof(BandName)}.{nameof(IsTooLong)}",
            "Название музыкальной группы слишком длинное.");
    }
}