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
        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileNameException"/>".
        /// </summary>
        public static TrackFileNameException IsNullOrWhiteSpace => new("Название файла музыкального трека не задано.");

        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileNameException"/>".
        /// </summary>
        public static TrackFileNameException IsTooShort => new("Название файла музыкального трека слишком короткое.");

        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileNameException"/>".
        /// </summary>
        public static TrackFileNameException IsTooLong => new("Название файла музыкального трека слишком длинное.");

        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileNameException"/>".
        /// </summary>
        public static TrackFileNameException Invalid => new("Некорректное название файла музыкального трека.");
    }

    /// <summary>
    /// Исключения MIME-типа файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileMimeTypeExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileMimeTypeException"/>".
        /// </summary>
        public static TrackFileMimeTypeException IsNullOrWhiteSpace =>
            new("MIME-тип файла музыкального трека не задан.");

        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileExtensionException"/>".
        /// </summary>
        public static TrackFileMimeTypeException Invalid => new("Некорректный MIME-тип файла музыкального трека.");
    }

    /// <summary>
    /// Исключения расширения файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileExtensionExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileExtensionException"/>".
        /// </summary>
        public static TrackFileExtensionException IsNullOrWhiteSpace =>
            new("Расширение файла музыкального трека не задано.");

        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileExtensionException"/>".
        /// </summary>
        public static TrackFileExtensionException Invalid => new("Некорректное расширение файла музыкального трека.");
    }

    /// <summary>
    /// Исключения содержимого файла доменной модели музыкального трека.
    /// </summary>
    public static class TrackFileDataExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileDataException"/>".
        /// </summary>
        public static TrackFileDataException IsEmpty => new("Файл музыкального трека пуст.");
    }
    
    /// <summary>
    /// Исключения продолжительности доменной модели файла музыкального трека.
    /// </summary>
    public static class TrackFileDurationExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="TrackFileDurationException"/>".
        /// </summary>
        public static TrackFileDurationException IncorrectDuration => new("Некорректная продолжительность файла музыкального трека.");
    }
}