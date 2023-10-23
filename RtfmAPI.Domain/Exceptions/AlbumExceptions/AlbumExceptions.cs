namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключения доменной модели музыкального альбома.
/// </summary>
public static class AlbumExceptions
{
    /// <summary>
    /// Исключения названия доменной модели музыкального альбома.
    /// </summary>
    public static class AlbumNameExceptions
    {
        /// <inheritdoc cref="AlbumNameException"/>
        public static AlbumNameException IsNullOrWhiteSpace => new("Название музыкального альбома не задано.");

        /// <inheritdoc cref="AlbumNameException"/>
        public static AlbumNameException IsTooShort => new("Название музыкального альбома слишком короткое.");

        /// <inheritdoc cref="AlbumNameException"/>
        public static AlbumNameException IsTooLong => new("Название музыкального альбома слишком длинное.");
    }

    /// <summary>
    /// Исключения даты выпуска доменной модели музыкального альбома.
    /// </summary>
    public static class AlbumReleaseDateExceptions
    {
        /// <inheritdoc cref="AlbumReleaseDateException"/>
        public static AlbumReleaseDateException InvalidDate => new("Некорректная дата выпуска музыкального альбома.");
    }
}