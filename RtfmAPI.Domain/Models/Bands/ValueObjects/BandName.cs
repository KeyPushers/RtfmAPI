using RftmAPI.Domain.Errors.BandErrors;
using RftmAPI.Domain.Primitives;
using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Models.Bands.ValueObjects;

/// <summary>
/// Название музыкальной группы.
/// </summary>
public sealed class BandName : ValueObject
{
    /// <summary>
    /// Минимальная длина названия.
    /// </summary>
    public const int MinLength = 1;

    /// <summary>
    /// Максимальная длина названия.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Название музыкальной группы.
    /// </summary>
    /// <param name="value">Значение.</param>
    private BandName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия музыкальной группы.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название музыкальной группы.</returns>
    public static Result<BandName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<BandName>.Failure(BandErrors.BandName.IsNullOrWhiteSpace);
        }

        if (value.Length < MinLength)
        {
            return Result<BandName>.Failure(BandErrors.BandName.IsTooShort);
        }

        if (value.Length > MaxLength)
        {
            return Result<BandName>.Failure(BandErrors.BandName.IsTooLong);
        }
        
        return new BandName(value);
    }
    
    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}