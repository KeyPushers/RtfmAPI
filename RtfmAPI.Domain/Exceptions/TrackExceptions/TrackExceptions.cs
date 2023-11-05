using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключения доменной модели музыкального трека.
/// </summary>
public static class TrackExceptions
{
    /// <summary>
    /// Создание "<inheritdoc cref="TrackNotFoundException"/>".
    /// </summary>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    public static TrackNotFoundException NotFound(TrackId trackId) => new($"Не удалось найти музыкальный трек с идентификатором [{trackId.Value}].");
    
    /// <summary>
    /// Исключения названия доменной модели музыкального трека.
    /// </summary>
    public static class TrackNameExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="TrackNameException"/>".
        /// </summary>
        public static TrackNameException IsNullOrWhiteSpace => new("Название музыкального трека не задано.");

        /// <summary>
        /// Создание "<inheritdoc cref="TrackNameException"/>".
        /// </summary>
        public static TrackNameException IsTooShort => new("Название музыкального трека слишком короткое.");

        /// <summary>
        /// <inheritdoc cref="TrackNameException"/>.
        /// </summary>
        public static TrackNameException IsTooLong => new("Название музыкального трека слишком длинное.");
    }

    /// <summary>
    /// Исключения даты выпускадоменной модели музыкального трека.
    /// </summary>
    public static class TrackReleaseDateExceptions
    {
        /// <summary>
        /// <inheritdoc cref="TrackReleaseDateException"/>.
        /// </summary>
        public static TrackReleaseDateException InvalidDate => new("Некорректная дата выпуска музыкального трека.");
    }
}