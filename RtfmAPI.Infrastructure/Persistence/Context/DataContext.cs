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
        const string sql = @"
                        CREATE TABLE IF NOT EXISTS bands (
                        id UUID PRIMARY KEY,
                        name VARCHAR
                    );
                    ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitAlbumsTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"CREATE TABLE IF NOT EXISTS albums (
                        id UUID PRIMARY KEY,
                        name VARCHAR,
                        released_date Date)";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitBandAlbumsTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"
                    CREATE TABLE IF NOT EXISTS bands_albums (
                        CONSTRAINT band_album_id PRIMARY KEY (band_id, album_id),
                        band_id UUID REFERENCES bands(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        album_id UUID REFERENCES albums(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitGenresTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"CREATE TABLE IF NOT EXISTS genres (
                        id UUID PRIMARY KEY,
                        name VARCHAR)";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitBandGenresTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"
                    CREATE TABLE IF NOT EXISTS bands_genres (
                        CONSTRAINT band_genre_id PRIMARY KEY (band_id, genre_id),
                        band_id UUID REFERENCES bands(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        genre_id UUID REFERENCES genres(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitTrackFilesTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"CREATE TABLE IF NOT EXISTS trackfiles (
                        id UUID PRIMARY KEY,
                        name VARCHAR,
                        data BYTEA,
                        extension VARCHAR,
                        mime_type VARCHAR,
                        duration DOUBLE PRECISION);
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitTracksTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"CREATE TABLE IF NOT EXISTS tracks (
                        id UUID PRIMARY KEY,
                        name VARCHAR,
                        release_date Date,
                        trackfile_id UUID REFERENCES trackfiles(Id) ON UPDATE CASCADE ON DELETE SET NULL);
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitAlbumTracksTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"
                    CREATE TABLE IF NOT EXISTS albums_tracks (
                        CONSTRAINT album_track_id PRIMARY KEY (album_Id, track_id),
                        album_Id UUID REFERENCES albums(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        track_id UUID REFERENCES tracks(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }

    private static Task InitTrackGenresTableAsync(IDbConnection connection, IDbTransaction trx)
    {
        const string sql = @"
                    CREATE TABLE IF NOT EXISTS tracks_genres (
                        CONSTRAINT track_genre_id PRIMARY KEY (track_Id, genre_Id),
                        track_Id UUID REFERENCES tracks(Id) ON UPDATE CASCADE ON DELETE CASCADE,
                        genre_Id UUID REFERENCES genres(Id) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                  ";

        return connection.ExecuteAsync(sql, trx);
    }
}