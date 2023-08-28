using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Bands.ValueObjects;

/// <summary>
/// Наименование музыкальной группы
/// </summary>
public class BandName : ValueObject
{
    private const int MinLength = 2;
    private const int MaxLength = 50;
    
    /// <summary>
    /// Наименование музыкальной группы
    /// </summary>
    /// <param name="name">Имя</param>
    public BandName(string name)
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
}