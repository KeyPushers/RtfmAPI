using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using RtfmAPI.Domain.Primitives;

namespace RtfmAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Базовый репозиторий команд.
/// </summary>
public abstract class BaseCommandsRepository
{
    /// <summary>
    /// Вызов обработчика.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="entityWithDomainEvents">Доменная сущность.</param>
    /// <param name="domainEventsHandler">Обработчик доменных событий.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected static Task InvokeAsync(IDbConnection connection, IDbTransaction transaction,
        IHasDomainEvents entityWithDomainEvents,
        Func<IDbConnection, IDbTransaction, IDomainEvent, Task> domainEventsHandler,
        CancellationToken cancellationToken = default)
    {
        return InvokeAsync<IDomainEvent>(connection, transaction, entityWithDomainEvents, domainEventsHandler,
            cancellationToken);
    }
    
    /// <summary>
    /// Вызов обработчика.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="entityWithDomainEvents">Доменная сущность.</param>
    /// <param name="domainEventsHandler">Обработчик доменных событий.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <typeparam name="TDomainEvent">Тип доменного события.</typeparam>
    protected static async Task InvokeAsync<TDomainEvent>(IDbConnection connection, IDbTransaction transaction,
        IHasDomainEvents entityWithDomainEvents,
        Func<IDbConnection, IDbTransaction, TDomainEvent, Task> domainEventsHandler,
        CancellationToken cancellationToken = default) where TDomainEvent: IDomainEvent
    {
        foreach (var domainEvent in entityWithDomainEvents.DomainEvents)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await domainEventsHandler(connection, transaction, (TDomainEvent) domainEvent);
        }

        entityWithDomainEvents.ClearDomainEvents();
    }
}