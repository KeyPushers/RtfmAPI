using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Band.ValueObjects;

/// <summary>
/// Название музыкальной группы.
/// </summary>
public class BandName : ValueObject
{
    private const int MinLength = 1;
    private const int MaxLength = 50;
    
    /// <summary>
    /// Название альбома.
    /// </summary>
    /// <param name="name">Имя.</param>
    public BandName(string name)
    {
        SetName(name);
    }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; private set; } = null!;
    
    /// <summary>
    /// Установка наименования.
    /// </summary>
    /// <param name="name">Наименование</param>
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
    /// Метод сравнения.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}