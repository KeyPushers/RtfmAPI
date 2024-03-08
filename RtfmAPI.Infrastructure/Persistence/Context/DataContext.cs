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
        await InitBandGenresTableAsync(connection, trx);
        await InitTrackFilesTableAsync(connection, trx);
        await InitTracksTableAsync(connection, trx);
        await InitAlbumTracksTableAsync(connection, trx);
        await InitTrackGenresTableAsync(connection, trx);
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
                        CONSTRAINT BandAlbumId PRIMARY KEY (BandId, AlbumId),
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

    private static Task InitBandGenresTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"
                    CREATE TABLE IF NOT EXISTS BandGenres (
                        CONSTRAINT BandGenreId PRIMARY KEY (BandId, GenreId),
                        BandId UUID REFERENCES Bands(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        GenreId UUID REFERENCES Genres(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitTrackFilesTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"CREATE TABLE IF NOT EXISTS TrackFiles (
                        Id UUID PRIMARY KEY,
                        Name VARCHAR,
                        Data BYTEA,
                        Extension VARCHAR,
                        MimeType VARCHAR,
                        Duration DOUBLE PRECISION);
                  ";

        return connection.ExecuteAsync(sql, trx);
    }
    
    private static Task InitTracksTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"CREATE TABLE IF NOT EXISTS Tracks (
                        Id UUID PRIMARY KEY,
                        Name VARCHAR,
                        ReleaseDate Date,
                        TrackFileId UUID REFERENCES TrackFiles(Id) ON UPDATE CASCADE ON DELETE CASCADE);
                  ";

        return connection.ExecuteAsync(sql, trx);
    }
    
    private static Task InitAlbumTracksTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"
                    CREATE TABLE IF NOT EXISTS AlbumTracks (
                        CONSTRAINT AlbumTrackId PRIMARY KEY (AlbumId, TrackId),
                        AlbumId UUID REFERENCES Albums(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        TrackId UUID REFERENCES Tracks(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }
    
    private static Task InitTrackGenresTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        var sql = @"
                    CREATE TABLE IF NOT EXISTS TrackGenres (
                        CONSTRAINT TrackGenreId PRIMARY KEY (TrackId, GenreId),
                        TrackId UUID REFERENCES Tracks(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        GenreId UUID REFERENCES Genres(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }
}