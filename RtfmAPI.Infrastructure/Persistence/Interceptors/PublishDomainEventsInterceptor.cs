using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RftmAPI.Domain.Primitives;

namespace RtfmAPI.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Перехватчик доменных событий.
/// </summary>
public class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _mediator;

    /// <summary>
    /// Создание перехватчика доменных событий.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public PublishDomainEventsInterceptor(IPublisher mediator)
    {
        _mediator = mediator;
    }
    
    /// <inheritdoc cref="SaveChangesInterceptor.SavingChanges"/>
    [Obsolete($"Применитять асинхронную реализацию: {nameof(SavingChangesAsync)}")]
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PublishDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc cref="SaveChangesInterceptor.SavingChangesAsync"/>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await PublishDomainEventsAsync(eventData.Context);
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Публикация доменных событий.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    private async Task PublishDomainEventsAsync(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }
        
        // Получение всех сущностей, содержащих доменные события.
        var entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity).ToList();
        
        // Получение доменных событий сущностей.
        var domainEvents = entitiesWithDomainEvents
            .SelectMany(entry => entry.DomainEvents).ToList();

        // Очистка доменных событий в сущностях, после их получения.
        entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());
        
        // Публикация доменных событий.
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}