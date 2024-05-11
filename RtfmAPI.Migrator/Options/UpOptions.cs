using CommandLine;

namespace RtfmAPI.Migrator.Options;

[Verb("up", HelpText = "Update database.")]
public class UpOptions : DbSettingsOptions
{
    [Option(Default = null, Required = false, HelpText = "Update up to specified migration number(included).")]
    public int? UpTo { get; set; }
}