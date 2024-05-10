using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RtfmAPI.Application.Interfaces.Persistence.Commands;
using RtfmAPI.Domain.Models.Genres;
using RtfmAPI.Domain.Models.Genres.Events;
using RtfmAPI.Infrastructure.Persistence.Context;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Репозиторий команд доменной модели <see cref="Genre"/>.
/// </summary>
public class GenreCommandsRepository : BaseCommandsRepository, IGenresCommandsRepository
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Создание репозитория команд доменной модели <see cref="Genre"/>.
    /// </summary>
    /// <param name="dataContext">Контекст базы данных.</param>
    public GenreCommandsRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc/>
    public async Task CommitChangesAsync(Genre value, CancellationToken cancellationToken)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var transaction = connection.BeginTransaction();

        await InvokeAsync<IGenreDomainEvent>(connection, transaction, value, GenreDomainEventsHandlerAsync,
            cancellationToken);

        transaction.Commit();
    }

    /// <summary>
    /// Обработчик событий музыкального жанра.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие.</param>
    private static Task GenreDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IGenreDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            GenreCreatedDomainEvent dEvent => OnGenreCreatedDomainEventAsync(dEvent, connection, transaction),
            GenreNameChangedDomainEvent dEvent => OnGenreNameChangedDomainEventAsync(dEvent, connection, transaction),
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };
    }

    /// <summary>
    /// Добавление музыкального жанра.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnGenreCreatedDomainEventAsync(GenreCreatedDomainEvent domainEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        var genre = domainEvent.Genre;

        const string sql = @"INSERT INTO genres (id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = genre.Id.Value, Name = string.Empty}, transaction);
    }

    /// <summary>
    /// Изменение названия музыкального жанра.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnGenreNameChangedDomainEventAsync(GenreNameChangedDomainEvent domainEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        var genre = domainEvent.Genre;

        const string sql = @"UPDATE genres SET name = @Name WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = genre.Id.Value, Name = genre.Name.Value}, transaction);
    }
}