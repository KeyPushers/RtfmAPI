namespace RtfmAPI.Domain.Models.Tracks.Exceptions;

internal static class TrackExceptions
{
    #region TrackNameExceptions

    internal static TrackNameException TrackNameIsNullOrEmpty() =>
        new("Не определено название музыкального трека.");

    internal static TrackNameException TrackNameIsTooShort() =>
        new("Название музыкального трека слишком короткое.");

    internal static TrackNameException TrackNameIsTooLong() =>
        new("Название музыкального трека слишком длинное.");

    #endregion

    #region TrackReleaseDateExceptions

    internal static TrackNameException InvalidTrackReleaseDate() =>
        new("Не определена дата выпуска музыкального трека");

    #endregion
}