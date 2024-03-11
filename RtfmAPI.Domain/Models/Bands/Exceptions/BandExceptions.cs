namespace RtfmAPI.Domain.Models.Bands.Exceptions;

internal static class BandExceptions
{
    #region BandNameException

    internal static BandNameException BandNameIsNullOrEmpty() =>
        new("Не определено название музыкальной группы.");

    internal static BandNameException BandNameIsTooShort() =>
        new("Название музыкальной группы слишком короткое.");

    internal static BandNameException BandNameIsTooLong() =>
        new("Название музыкальной группы слишком длинное.");
    
    #endregion
}