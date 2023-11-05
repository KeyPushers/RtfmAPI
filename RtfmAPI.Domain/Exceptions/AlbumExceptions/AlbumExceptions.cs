using RftmAPI.Domain.Models.Albums.ValueObjects;
using RftmAPI.Domain.Models.Tracks.ValueObjects;

namespace RftmAPI.Domain.Exceptions.AlbumExceptions;

/// <summary>
/// Исключения доменной модели музыкального альбома.
/// </summary>
public static class AlbumExceptions
{
    /// <summary>
    /// Создание "<inheritdoc cref="AlbumDoesntContainTrackException"/>".
    /// </summary>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    public static AlbumDoesntContainTrackException AlbumDoesntContainTrackException(AlbumId albumId, TrackId trackId) =>
        new($"Музыкальный альбом [{albumId}] не содержит музыкальный трек [{trackId}].");

    /// <summary>
    /// Создание "<inheritdoc cref="AlbumNotFoundException"/>".
    /// </summary>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    public static AlbumNotFoundException NotFound(AlbumId albumId) =>
        new($"Не удалось найти музыкальный альбом с идентификатором [{albumId.Value}].");

    /// <summary>
    /// Создание "<inheritdoc cref="TrackRemovingFromAlbumFailedException"/>".
    /// </summary>
    /// <param name="albumId">Идентификатор музыкального альбома.</param>
    /// <param name="trackId">Идентификатор музыкального трека.</param>
    public static TrackRemovingFromAlbumFailedException
        TrackRemovingFromAlbumFailedException(AlbumId albumId, TrackId trackId) =>
        new($"Не удалось удалить из музыкального альбома [{trackId}] музыкальный трек [{albumId}].");

    /// <summary>
    /// Исключения названия доменной модели музыкального альбома.
    /// </summary>
    public static class AlbumNameExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="AlbumNameException"/>".
        /// </summary>
        public static AlbumNameException IsNullOrWhiteSpace => new("Название музыкального альбома не задано.");

        /// <summary>
        /// Создание <inheritdoc cref="AlbumNameException"/>".
        /// </summary>
        public static AlbumNameException IsTooShort => new("Название музыкального альбома слишком короткое.");

        /// <summary>
        /// Создание <inheritdoc cref="AlbumNameException"/>".
        /// </summary>
        public static AlbumNameException IsTooLong => new("Название музыкального альбома слишком длинное.");
    }

    /// <summary>
    /// Исключения даты выпуска доменной модели музыкального альбома.
    /// </summary>
    public static class AlbumReleaseDateExceptions
    {
        /// <summary>
        /// Создание "<inheritdoc cref="AlbumReleaseDateException"/>".
        /// </summary>
        public static AlbumReleaseDateException InvalidDate => new("Некорректная дата выпуска музыкального альбома.");
    }
}