using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Models.Tracks.ValueObjects;

/// <summary>
/// Идентификатор файла музыкального трека.
/// </summary>
public sealed class TrackFileId : ValueObject
{
    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Идентификатор файла музыкального трека.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private TrackFileId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание идентификатора файла музыкального трека.
    /// </summary>
    /// <returns>Идентификатор файла музыкального трека.</returns>
    public static TrackFileId Create()
    {
        return new TrackFileId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора файла музыкального трека.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Идентификатор файла музыкального трека.</returns>
    public static TrackFileId Create(Guid id)
    {
        return new TrackFileId(id);
    }

    /// <inheritdoc cref="ValueObject.GetEqualityComponents"/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}