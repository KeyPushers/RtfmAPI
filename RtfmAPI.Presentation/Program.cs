using Microsoft.AspNetCore.Builder;
using Pilotiv.AuthorizationAPI.Jwt;
using RtfmAPI.Application;
using RtfmAPI.Infrastructure;
using RtfmAPI.Presentation;
using RtfmAPI.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseHostExtensions();

builder.AddJwtProviderAuthentication();
builder.AddApplication();
builder.AddInfrastructure();
builder.AddPresentation();
builder.ConfigureOptions();
    
var app = builder.Build();

app.UseWebApplicationExtension();

app.Run();