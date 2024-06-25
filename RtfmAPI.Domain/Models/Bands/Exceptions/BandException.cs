using RtfmAPI.Domain.Exceptions;

namespace RtfmAPI.Domain.Models.Bands.Exceptions;

internal abstract class BandException : DomainException
{
    internal BandException(string message) : base($"{nameof(BandException)}.{message}")
    {
    }
}