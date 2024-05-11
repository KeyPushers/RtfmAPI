using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using RtfmAPI.Migrator.Options;

namespace RtfmAPI.Migrator;

public static class MigratorRunner
{
    public static RunStatus CheckDatabase(IServiceProvider serviceProvider, CheckOptions opts)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        return TryRun(runner.ValidateVersionOrder);
    }

    public static RunStatus UpDatabase(IServiceProvider serviceProvider, UpOptions opts)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        if (opts.UpTo.HasValue)
        {
            return TryRun(() => runner.MigrateUp(opts.UpTo.Value));
        }
    
        return TryRun(runner.MigrateUp);
    }
    
    public static RunStatus DownDatabase(IServiceProvider serviceProvider, DownOptions opts)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        return TryRun(() => runner.MigrateDown(opts.DownTo));
    }
    
    private static RunStatus TryRun(Action action)
    {
        try
        {
            action();
            return RunStatus.Ok;
        }
        catch (Exception)
        {
            return RunStatus.Error;
        }
    }
}