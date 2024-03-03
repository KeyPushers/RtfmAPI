using Microsoft.Extensions.Options;
using Dapper;
using Npgsql;
using System.Data;
using System.Threading.Tasks;
using RtfmAPI.Infrastructure.Data;

namespace RtfmAPI.Infrastructure.Persistence.Context;

/// <summary>
/// Основной контекст базы данных.
/// </summary>
public class DataContext
{
    private readonly DbSettingsOptions _dbSettingsOptions;

    /// <summary>
    /// Создание основного контекста базы данных.
    /// </summary>
    /// <param name="dbSettings"></param>
    public DataContext(IOptions<DbSettingsOptions> dbSettings)
    {
        _dbSettingsOptions = dbSettings.Value;
    }
    
    /// <summary>
    /// Создание соединения с базой данных..
    /// </summary>
    private IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_dbSettingsOptions.Server}; Database={_dbSettingsOptions.Database}; Username={_dbSettingsOptions.UserId}; Password={_dbSettingsOptions.Password};";
        return new NpgsqlConnection(connectionString);
    }

    /// <summary>
    /// Инициализация соединения.
    /// </summary>
    public async Task InitAsync()
    {
        await InitDatabaseAsync();
        await InitTablesAsync();
    }

    /// <summary>
    /// Инициализация базы данных.
    /// </summary>
    private async Task InitDatabaseAsync()
    {
        // create database if it doesn't exist
        var connectionString = $"Host={_dbSettingsOptions.Server}; Database=postgres; Username={_dbSettingsOptions.UserId}; Password={_dbSettingsOptions.Password};";
        await using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettingsOptions.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettingsOptions.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }
    
    /// <summary>
    /// Инициализация таблиц.
    /// </summary>
    private async Task InitTablesAsync()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await InitUsers();

        async Task InitUsers()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR,
                    FirstName VARCHAR,
                    LastName VARCHAR,
                    Email VARCHAR,
                    Role INTEGER,
                    PasswordHash VARCHAR
                );
            """;
            await connection.ExecuteAsync(sql);
        }
    }
}