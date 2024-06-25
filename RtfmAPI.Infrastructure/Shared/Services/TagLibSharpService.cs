using System.IO;
using RtfmAPI.Application.Interfaces.AudioHandlers;
using TagLib;
using File = TagLib.File;

namespace RtfmAPI.Infrastructure.Shared.Services;

/// <summary>
/// Обработчик аудиофайла "TagLib Sharp".
/// </summary>
public class TagLibSharpService : IAudioHandler
{
    private readonly File _service;

    /// <summary>
    /// Создание обработчика аудиофайла "TagLib Sharp".
    /// </summary>
    /// <param name="stream">Поток аудиофайла.</param>
    /// <param name="mimetype">MIME-тип файла.</param>
    public TagLibSharpService(Stream stream, string mimetype)
    {
        var fileAbstraction = new FileAbstraction(stream);

        _service = File.Create(fileAbstraction, mimetype, ReadStyle.Average);
    }

    /// <summary>
    /// <inheritdoc cref="IAudioHandler.GetDuration"/>
    /// </summary>
    /// <returns>Продолжительность аудиофайла в миллисекундах.</returns>
    public double GetDuration()
    {
        return _service.Properties.Duration.TotalMilliseconds;
    }

    /// <summary>
    /// Абстракция файла для считывания библиотекой.
    /// </summary>
    private class FileAbstraction : File.IFileAbstraction
    {
        /// <summary>
        /// Создание абстракции файла.
        /// </summary>
        /// <param name="stream">Поток аудиофайла.</param>
        public FileAbstraction(Stream stream)
        {
            stream.Position = 0;
            Name = string.Empty;
            ReadStream = stream;
            WriteStream = stream;
        }

        public void CloseStream(Stream stream)
        {
            stream.Position = 0;
        }

        public string Name { get; }
        public Stream ReadStream { get; }
        public Stream WriteStream { get; }
    }
}