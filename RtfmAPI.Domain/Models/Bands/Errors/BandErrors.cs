using FluentResults;

namespace RtfmAPI.Domain.Models.Bands.Errors;

internal static class BandErrors
{
    #region BandNameException

    internal static Error BandNameIsNullOrEmpty() =>
        new("Не определено название музыкальной группы.");

    internal static Error BandNameIsTooShort() =>
        new("Название музыкальной группы слишком короткое.");

    internal static Error BandNameIsTooLong() =>
        new("Название музыкальной группы слишком длинное.");
    
    #endregion
}