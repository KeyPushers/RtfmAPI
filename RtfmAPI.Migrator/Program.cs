using System;
using System.Reflection;
using CommandLine;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using RtfmAPI.Migrator;
using RtfmAPI.Migrator.Options;

return Parser.Default.ParseArguments<CheckOptions, UpOptions, DownOptions>(args)
    .MapResult(
        (CheckOptions options) => HandleCheckOptions(options), 
        (UpOptions options) => HandleUpOptions(options), 
        (DownOptions options) => HandleDownOptions(options),
        _ => 1);

static int HandleCheckOptions(CheckOptions options)
{
    var serviceProvider = CreateServices(options.ConnectionString);
    using var scope = serviceProvider.CreateScope();
    
    return (int) MigratorRunner.CheckDatabase(scope.ServiceProvider, options);
}

static int HandleUpOptions(UpOptions options)
{
    var serviceProvider = CreateServices(options.ConnectionString);
    using var scope = serviceProvider.CreateScope();
    
    return (int) MigratorRunner.UpDatabase(scope.ServiceProvider, options);
}

static int HandleDownOptions(DownOptions options)
{
    var serviceProvider = CreateServices(options.ConnectionString);
    using var scope = serviceProvider.CreateScope();
    
    return (int) MigratorRunner.DownDatabase(scope.ServiceProvider, options);
}

static IServiceProvider CreateServices(string connectionString)
{
    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}