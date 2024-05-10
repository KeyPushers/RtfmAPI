using FluentResults;

namespace RtfmAPI.Domain.Models.Albums.Errors;

internal static class AlbumErrors
{
    #region AlbumNameException

    internal static Error AlbumNameIsNullOrEmpty() =>
        new("Не определено название музыкального альбома.");

    internal static Error AlbumNameIsTooShort() =>
        new("Название музыкального альбома слишком короткое.");

    internal static Error AlbumNameIsTooLong() =>
        new("Название музыкального альбома слишком длинное.");

    #endregion

    #region AlbumReleaseDateException

    internal static Error AlbumNameReleaseDateIsInvalid() =>
        new("Не определена дата выпуска музыкального альбома.");

    #endregion
}