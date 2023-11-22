using RftmAPI.Domain.Models.Bands.ValueObjects;

namespace RftmAPI.Domain.Exceptions.BandExceptions;

/// <summary>
/// Исключения доменной модели музыкальной группы.
/// </summary>
public static class BandExceptions
{
    /// <summary>
    /// Создание "<inheritdoc cref="BandNotFoundException"/>".
    /// </summary>
    /// <param name="bandId">Идентификатор музыкальной группы.</param>
    public static BandNotFoundException NotFound(BandId bandId) =>
        new($"Не удалось найти музыкальную группу с идентификатором [{bandId.Value}].");
    
    /// <summary>
    /// Исключения названия доменной модели музыкальной группы.
    /// </summary>
    public static class BandNameExceptions
    {
        /// <inheritdoc cref="BandNameException"/>
        public static BandNameException IsNullOrWhiteSpace => new("Название музыкальной группы не задано.");

        /// <inheritdoc cref="BandNameException"/>
        public static BandNameException IsTooShort => new("Название музыкальной группы слишком короткое.");

        /// <inheritdoc cref="BandNameException"/>
        public static BandNameException IsTooLong => new("Название музыкальной группы слишком длинное.");
    }
}