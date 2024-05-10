using FluentResults;

namespace RtfmAPI.Domain.Models.Tracks.Errors;

internal static class TrackErrors
{
    #region TrackNameExceptions

    internal static Error TrackNameIsNullOrEmpty() =>
        new("Не определено название музыкального трека.");

    internal static Error TrackNameIsTooShort() =>
        new("Название музыкального трека слишком короткое.");

    internal static Error TrackNameIsTooLong() =>
        new("Название музыкального трека слишком длинное.");

    #endregion

    #region TrackReleaseDateException

    internal static Error InvalidTrackReleaseDate() =>
        new("Не определена дата выпуска музыкального трека");

    #endregion
}