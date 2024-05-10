using FluentResults;

namespace RtfmAPI.Domain.Models.TrackFiles.Errors;

internal static class TrackFileErrors
{
    #region TrackFileDataException

    internal static Error TrackFileDataIsEmpty() =>
        new("Отсутствуют данные в файле музыкального трека");
    
    #endregion

    #region TrackFileDurationException

    internal static Error TrackFileDurationIsInvalid() =>
        new("Ошибка в значении величины продолжительности файла музыкального трека");
    
    #endregion

    #region TrackFileExtensionException

    internal static Error TrackFileExtensionIsNullOrEmpty() =>
        new("Не определено расширение файла музыкального трека.");

    internal static Error TrackFileExtensionIsInvalid() =>
        new("Ошибка в расширении файла музыкального трека.");
    
    #endregion

    #region TrackFileMimeTypeException

    internal static Error TrackFileMimeTypeIsNullOrEmpty() =>
        new("Не определен MIME-тип файла музыкального трека.");

    internal static Error TrackFileMimeTypeUnknown() =>
        new("Неизвестный MIME-тип файла музыкального трека.");

    #endregion

    #region TrackFileNameException

    internal static Error TrackFileNameIsNullOrEmpty() =>
        new("Не определено название файла музыкального трека.");

    internal static Error TrackFileNameIsTooShort() =>
        new("Название файла музыкального трека слишком короткое.");

    internal static Error TrackFileNameIsTooLong() =>
        new("Название файла музыкального трека слишком длинное.");

    internal static Error TrackFileNameIsInvalid() =>
        new("Ошибка в названии файла музыкального трека.");

    #endregion
}