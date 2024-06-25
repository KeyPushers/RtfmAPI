namespace RtfmAPI.Domain.Models.Albums.Exceptions;

internal static class AlbumExceptions
{
    #region AlbumNameException

    internal static AlbumNameException AlbumNameIsNullOrEmpty() =>
        new("Не определено название музыкального альбома.");

    internal static AlbumNameException AlbumNameIsTooShort() =>
        new("Название музыкального альбома слишком короткое.");

    internal static AlbumNameException AlbumNameIsTooLong() =>
        new("Название музыкального альбома слишком длинное.");

    #endregion

    #region AlbumReleaseDateException

    internal static AlbumReleaseDateException AlbumNameReleaseDateIsInvalid() =>
        new("Не определена дата выпуска музыкального альбома.");

    #endregion
}