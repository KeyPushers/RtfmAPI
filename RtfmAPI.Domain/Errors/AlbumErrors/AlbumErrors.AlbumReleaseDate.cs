using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.AlbumErrors;

/// <summary>
/// Ошибки доменной модели музыкального альбома.
/// </summary>
public static partial class AlbumErrors
{
    /// <summary>
    /// Ошибки наименования музыкального альбома.
    /// </summary>
    public static class AlbumReleaseDate
    {
        /// <summary>
        /// Ошибка некорректной даты.
        /// </summary>
        public static readonly Error InvalidDate =
            new($"{nameof(AlbumErrors)}.{nameof(AlbumReleaseDate)}.{nameof(InvalidDate)}", "Некорректная дата.");
    }
}