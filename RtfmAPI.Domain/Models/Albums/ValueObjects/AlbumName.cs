using RftmAPI.Domain.Exceptions.AlbumExceptions;
using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Albums.ValueObjects;

/// <summary>
/// Название музыкального альбома.
/// </summary>
public sealed class AlbumName : ValueObject
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
    /// Название альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    private AlbumName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Название музыкального альбома.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание названия музыкального альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Название музыкального альбома.</returns>
    public static Result<AlbumName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return AlbumExceptions.AlbumNameExceptions.IsNullOrWhiteSpace;
        }

        if (value.Length < MinLength)
        {
            return AlbumExceptions.AlbumNameExceptions.IsTooShort;
        }

        if (value.Length > MaxLength)
        {
            return AlbumExceptions.AlbumNameExceptions.IsTooLong;
        }

        return new AlbumName(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}