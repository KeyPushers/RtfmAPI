using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.TrackErrors;

/// <summary>
/// Ошибки доменной модели музыкального трека.
/// </summary>
public static partial class TrackErrors
{
    /// <summary>
    /// Ошибки даты выпуска музыкального трека.
    /// </summary>
    public static class TrackReleaseDate
    {
        /// <summary>
        /// Ошибка некорректной даты.
        /// </summary>
        public static readonly Error InvalidDate =
            new($"{nameof(AlbumErrors)}.{nameof(TrackReleaseDate)}.{nameof(InvalidDate)}", "Некорректная дата.");
    }
}