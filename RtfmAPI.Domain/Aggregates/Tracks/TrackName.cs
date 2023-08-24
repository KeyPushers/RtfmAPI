using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Tracks;

/// <summary>
/// Наименование альбома
/// </summary>
public sealed class TrackName: ValueObject
{
    private const int MinLength = 2;
    private const int MaxLength = 50;
    
    /// <summary>
    /// Наименование альбома
    /// </summary>
    /// <param name="name">Имя</param>
    public TrackName(string name)
    {
        SetName(name);
    }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; private set; } = null!;
    
    /// <summary>
    /// Установка наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <exception cref="NotImplementedException"></exception>
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new NotImplementedException();
        }

        if (name.Length is < MinLength or > MaxLength)
        {
            throw new NotImplementedException();
        }

        Name = name;
    }

    /// <summary>
    /// Максимальная длина наименования
    /// </summary>
    public static int NameMaxLength => MaxLength;
    
    /// <summary>
    /// Минимальная длина наименования
    /// </summary>
    public static int NameMinLength => MinLength;

}