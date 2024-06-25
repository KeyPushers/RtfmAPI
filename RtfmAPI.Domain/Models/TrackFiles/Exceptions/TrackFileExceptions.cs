namespace RtfmAPI.Domain.Models.TrackFiles.Exceptions;

internal static class TrackFileExceptions
{
    #region TrackFileDataException

    internal static TrackFileDataException TrackFileDataIsEmpty() =>
        new("Отсутствуют данные в файле музыкального трека");
    
    #endregion

    #region TrackFileDurationException

    internal static TrackFileDurationException TrackFileDurationIsInvalid() =>
        new("Ошибка в значении величины продолжительности файла музыкального трека");
    
    #endregion

    #region TrackFileExtensionException

    internal static TrackFileExtensionException TrackFileExtensionIsNullOrEmpty() =>
        new("Не определено расширение файла музыкального трека.");

    internal static TrackFileExtensionException TrackFileExtensionIsInvalid() =>
        new("Ошибка в расширении файла музыкального трека.");
    
    #endregion

    #region TrackFileMimeTypeException

    internal static TrackFileMimeTypeException TrackFileMimeTypeIsNullOrEmpty() =>
        new("Не определен MIME-тип файла музыкального трека.");

    internal static TrackFileMimeTypeException TrackFileMimeTypeUnknown() =>
        new("Неизвестный MIME-тип файла музыкального трека.");

    #endregion

    #region TrackFileNameException

    internal static TrackFileNameException TrackFileNameIsNullOrEmpty() =>
        new("Не определено название файла музыкального трека.");

    internal static TrackFileNameException TrackFileNameIsTooShort() =>
        new("Название файла музыкального трека слишком короткое.");

    internal static TrackFileNameException TrackFileNameIsTooLong() =>
        new("Название файла музыкального трека слишком длинное.");

    internal static TrackFileNameException TrackFileNameIsInvalid() =>
        new("Ошибка в названии файла музыкального трека.");

    #endregion
}