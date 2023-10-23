namespace RftmAPI.Domain.Exceptions.TrackExceptions;

/// <summary>
/// Исключения доменной модели музыкального трека.
/// </summary>
public static class TrackExceptions
{
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

    /// <summary>
    /// Исключения названия файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileNameExceptions
    {
        /// <inheritdoc cref="TrackFileNameException"/>
        public static TrackFileNameException IsNullOrWhiteSpace => new("Название файла музыкального трека не задано.");

        /// <inheritdoc cref="TrackFileNameException"/>
        public static TrackFileNameException IsTooShort => new("Название файла музыкального трека слишком короткое.");

        /// <inheritdoc cref="TrackFileNameException"/>
        public static TrackFileNameException IsTooLong => new("Название файла музыкального трека слишком длинное.");

        /// <inheritdoc cref="TrackFileNameException"/>
        public static TrackFileNameException Invalid => new("Некорректное название файла музыкального трека.");
    }

    /// <summary>
    /// Исключения MIME-типа файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileMimeTypeExceptions
    {
        /// <inheritdoc cref="TrackFileMimeTypeException"/>
        public static TrackFileMimeTypeException IsNullOrWhiteSpace =>
            new("MIME-тип файла музыкального трека не задан.");

        /// <inheritdoc cref="TrackFileMimeTypeException"/>
        public static TrackFileMimeTypeException Invalid => new("Некорректный MIME-тип файла музыкального трека.");
    }

    /// <summary>
    /// Исключения расширения файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileExtensionExceptions
    {
        /// <inheritdoc cref="TrackFileExtensionException"/>
        public static TrackFileExtensionException IsNullOrWhiteSpace =>
            new("Расширение файла музыкального трека не задано.");

        /// <inheritdoc cref="TrackFileExtensionException"/>
        public static TrackFileExtensionException Invalid => new("Некорректное расширение файла музыкального трека.");
    }

    /// <summary>
    /// Исключения содержимого файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileDataExceptions
    {
        /// <inheritdoc cref="TrackFileDataException"/>
        public static TrackFileDataException IsEmpty => new("Файл музыкального трека пуст.");
    }
}