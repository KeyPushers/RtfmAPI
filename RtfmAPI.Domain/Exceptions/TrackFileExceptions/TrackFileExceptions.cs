namespace RftmAPI.Domain.Exceptions.TrackFileExceptions;

/// <summary>
/// Исключения доменной модели файла музыкального трека.
/// </summary>
public static class TrackFileExceptions
{
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