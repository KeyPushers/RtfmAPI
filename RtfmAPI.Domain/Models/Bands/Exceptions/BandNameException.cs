namespace RtfmAPI.Domain.Models.Bands.Exceptions;

internal class BandNameException : BandException
{
    internal BandNameException(string message) : base($"{nameof(BandNameException)}.{message}")
    {
    }
}