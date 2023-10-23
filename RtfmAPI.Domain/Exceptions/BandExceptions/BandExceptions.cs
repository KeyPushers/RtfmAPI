namespace RftmAPI.Domain.Exceptions.BandExceptions;

/// <summary>
/// Исключения доменной модели музыкальной группы.
/// </summary>
public static class BandExceptions
{
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