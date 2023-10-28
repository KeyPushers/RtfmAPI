using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtfmAPI.Application;
using RtfmAPI.Infrastructure;
using RtfmAPI.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseHostExtensions();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebApplicationExtensions();
app.MapControllers();

app.Run();