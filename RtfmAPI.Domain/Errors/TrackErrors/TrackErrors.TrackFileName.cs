using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors.TrackErrors;

/// <summary>
/// Ошибки доменной модели музыкального трека.
/// </summary>
public static partial class TrackErrors
{
    /// <summary>
    /// Ошибки названия файла музыкального трека.
    /// </summary>
    public static class TrackFileName
    {
        /// <summary>
        /// Ошибка незаданного наименования файла музыкального трека.
        /// </summary>
        public static readonly Error IsNullOrWhiteSpace = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileName)}.{nameof(IsNullOrWhiteSpace)}",
            "Название файла музыкального трека не задано.");
            
        /// <summary>
        /// Ошибка короткого наименования файла музыкального трека.
        /// </summary>
        public static readonly Error IsTooShort = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileName)}.{nameof(IsTooShort)}",
            "Название файла музыкального трека слишком короткое.");
            
        /// <summary>
        /// Ошибка длинного наименования файла музыкального трека.
        /// </summary>
        public static readonly Error IsTooLong = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileName)}.{nameof(IsTooLong)}",
            "Название файла музыкального трека слишком длинное.");
        
        /// <summary>
        /// Ошибка некорректного названия файла музыкального трека.
        /// </summary>
        public static readonly Error Invalid = new(
            $"{nameof(TrackErrors)}.{nameof(TrackFileName)}.{nameof(Invalid)}",
            "Некорректное название файла.");
    }
}