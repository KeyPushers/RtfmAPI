using RftmAPI.Domain.Shared;

namespace RftmAPI.Domain.Errors;

/// <summary>
/// Описание общих ошибок.
/// </summary>
public static class SharedErrors
{
    /// <summary>
    /// Отсутствие ошибка.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Ошибка пустого значения.
    /// </summary>
    public static readonly Error NullValue = new($"Error.{nameof(NullValue)}", "Значение не задано.");
}