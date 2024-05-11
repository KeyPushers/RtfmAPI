using CommandLine;

namespace RtfmAPI.Migrator.Options;

[Verb("check", isDefault: true, aliases: new[] {"ValidateVersionOrder"}, HelpText = "Validate version order.")]
public class CheckOptions : DbSettingsOptions
{
}