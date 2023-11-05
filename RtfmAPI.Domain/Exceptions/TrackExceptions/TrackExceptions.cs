namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключения доменной модели музыкального трека.
/// </summary>
public static class TrackExceptions
{
    /// <summary>
    /// Исключение добавления музыкального альбома к треку.
    /// </summary>
    public static AlbumIsAlreadyAddedToTrackException AlbumIsAlreadyAddedToTrackException => new("К треку уже привязан альбом.");
    
    /// <summary>
    /// Исключения названия доменной модели музыкального трека.
    /// </summary>
    public static class TrackNameExceptions
    {
        /// <inheritdoc cref="TrackNameException"/>
        public static TrackNameException IsNullOrWhiteSpace => new("Название музыкального трека не задано.");

        /// <inheritdoc cref="TrackNameException"/>
        public static TrackNameException IsTooShort => new("Название музыкального трека слишком короткое.");

        /// <inheritdoc cref="TrackNameException"/>
        public static TrackNameException IsTooLong => new("Название музыкального трека слишком длинное.");
    }

    /// <summary>
    /// Исключения даты выпускадоменной модели музыкального трека.
    /// </summary>
    public static class TrackReleaseDateExceptions
    {
        /// <inheritdoc cref="TrackReleaseDateException"/>
        public static TrackReleaseDateException InvalidDate => new("Некорректная дата выпуска музыкального трека.");
    }
}