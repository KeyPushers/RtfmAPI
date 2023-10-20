using RftmAPI.Domain.Errors.AlbumErrors;
using RftmAPI.Domain.Primitives;
using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Models.Albums.ValueObjects;

/// <summary>
/// Дата выпуска музыкального альбома.
/// </summary>
public class AlbumReleaseDate : ValueObject
{
    /// <summary>
    /// Дата выпуска музыкального альбома.
    /// </summary>
    /// <param name="value">Значение.</param>
    private AlbumReleaseDate(DateTime value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение.
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// Создание даты выпуска музыкального альбома.
    /// </summary>
    /// <param name="value">Дата.</param>
    /// <returns>Дата выпуска музыкального альбома.</returns>
    public static Result<AlbumReleaseDate> Create(DateTime value)
    {
        if (value == DateTime.MinValue)
        {
            return Result<AlbumReleaseDate>.Failure(AlbumErrors.AlbumReleaseDate.InvalidDate);
        }

        return new AlbumReleaseDate(value);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}