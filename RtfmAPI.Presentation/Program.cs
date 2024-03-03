using Microsoft.AspNetCore.Builder;
using RtfmAPI.Application;
using RtfmAPI.Infrastructure;
using RtfmAPI.Presentation;
using RtfmAPI.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseHostExtensions();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPresentation();
builder.Services.ConfigureOptions(builder);
    
var app = builder.Build();

app.UseWebApplicationExtension();

app.Run();