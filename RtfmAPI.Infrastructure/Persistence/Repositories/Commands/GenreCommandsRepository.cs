using System.Data;
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
public class GenreCommandsRepository : IGenresCommandsRepository
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
    public async Task CommitChangesAsync(Genre value)
    {
        using var connection = _dataContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in value.DomainEvents)
        {
            switch (domainEvent)
            {
                case GenreCreatedDomainEvent genreCreatedDomainEvent:
                {
                    await CreateGenreAsync(genreCreatedDomainEvent, connection, trx);
                    break;
                }
                
                case GenreNameChangedDomainEvent genreNameChangedDomainEvent:
                {
                    await ChangeGenreNameAsync(genreNameChangedDomainEvent, connection, trx);
                    break;
                }
            }
            
        }
        
        trx.Commit();
    }
    
    /// <summary>
    /// Добавление музыкального жанра.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task CreateGenreAsync(GenreCreatedDomainEvent domainEvent, IDbConnection connection, IDbTransaction trx)
    {
        var genre = domainEvent.Genre;

        var sql = """
                    INSERT INTO Genres (Id) VALUES(@Id)
                  """;
        return connection.ExecuteAsync(sql, new {Id = genre.Id.Value, Name = string.Empty}, trx);
    }
    
    /// <summary>
    /// Изменение названия музыкального жанра.
    /// </summary>
    /// <param name="domainEvent">Событие.</param>
    /// <param name="connection">Соединения.</param>
    /// <param name="trx">Транзакция.</param>
    private static Task ChangeGenreNameAsync(GenreNameChangedDomainEvent domainEvent, IDbConnection connection, IDbTransaction trx)
    {
        var genre = domainEvent.Genre;

        var sql = """
                    UPDATE Genres SET Name = @Name WHERE Id = @Id
                  """;
        return connection.ExecuteAsync(sql, new {Id = genre.Id.Value, Name = genre.Name.Value}, trx);
    }
}