namespace RftmAPI.Domain.Aggregates.Genres.ValueObjects;

/// <summary>
/// Наименование жанра музыки
/// </summary>
public class GenreName
{
    private const int MinLength = 2;
    private const int MaxLength = 50;
    
    /// <summary>
    /// Наименование жанра музыки
    /// </summary>
    /// <param name="name">Имя</param>
    public GenreName(string name)
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