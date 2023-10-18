using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Genre.ValueObjects;

/// <summary>
/// Название музыкального жанра.
/// </summary>
public class GenreName: ValueObject
{
    private const int MinLength = 1;
    private const int MaxLength = 50;
    
    /// <summary>
    /// Название жанра.
    /// </summary>
    /// <param name="name">Имя.</param>
    public GenreName(string name)
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