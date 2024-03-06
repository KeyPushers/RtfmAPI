using Microsoft.Extensions.Options;
using Dapper;
using Npgsql;
using System.Data;
using System.Threading.Tasks;
using RtfmAPI.Infrastructure.Data;

namespace RtfmAPI.Infrastructure.Persistence.Context;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class DataContext
{
    private readonly DbSettingsOptions _dbSettingsOptions;

    /// <summary>
    /// Создание контекста базы данных.
    /// </summary>
    /// <param name="dbSettings">Настройки подключения к базе данных.</param>
    public DataContext(IOptions<DbSettingsOptions> dbSettings)
    {
        _dbSettingsOptions = dbSettings.Value;
    }

    /// <summary>
    /// Создание открытого соединения с базой данных.
    /// </summary>
    public IDbConnection CreateOpenedConnection()
    {
        var connection = CreateConnection();
        connection.Open();
        return connection;
    }

    /// <summary>
    /// Создание соединения с базой данных.
    /// </summary>
    private IDbConnection CreateConnection()
    {
        var connectionString =
            $"Host={_dbSettingsOptions.Server}; Database={_dbSettingsOptions.Database}; Username={_dbSettingsOptions.UserId}; Password={_dbSettingsOptions.Password};";
        return new NpgsqlConnection(connectionString);
    }

    /// <summary>
    /// Инициализация соединения.
    /// </summary>
    public async Task InitAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        var trx = connection.BeginTransaction();

        await InitDatabaseAsync(connection, trx);
        await InitTablesAsync(connection, trx);

        trx.Commit();
    }

    /// <summary>
    /// Инициализация базы данных.
    /// </summary>
    private async Task InitDatabaseAsync(IDbConnection connection, IDbTransaction trx)
    {
        // create database if it doesn't exist
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettingsOptions.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount, trx);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettingsOptions.Database}\"";
            await connection.ExecuteAsync(sql, trx);
        }
    }

    /// <summary>
    /// Инициализация таблиц.
    /// </summary>
    private async Task InitTablesAsync(IDbConnection connection, IDbTransaction trx)
    {
        await InitBandsTableAsync(connection, trx);
        await InitAlbumsTableAsync(connection, trx);
        await InitBandAlbumsTableAsync(connection, trx);
        await InitGenresTableAsync(connection, trx);
    }

    private static Task InitBandsTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"
                        CREATE TABLE IF NOT EXISTS Bands (
                        Id UUID PRIMARY KEY,
                        Name VARCHAR
                    );
                    ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitAlbumsTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"CREATE TABLE IF NOT EXISTS Albums (
                        Id UUID PRIMARY KEY,
                        Name VARCHAR,
                        ReleaseDate Date)";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitBandAlbumsTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"
                    CREATE TABLE IF NOT EXISTS BandAlbums (
                        CONSTRAINT Id PRIMARY KEY (BandId, AlbumId),
                        BandId UUID REFERENCES Bands(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        AlbumId UUID REFERENCES Albums(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitGenresTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"CREATE TABLE IF NOT EXISTS Genres (
                        Id UUID PRIMARY KEY,
                        Name VARCHAR)";

        return connection.ExecuteAsync(sql, trx);
    }
}