using ApiTokenAzureStorage.Services;
using Scalar.AspNetCore;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ServiceSaSToken>();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

// NECESITAMOS MAPEAR NUESTRO METODO token DENTRO DE UN ENDPOINT
app.MapGet("/token/{curso}", (string curso, ServiceSaSToken service) =>
{
    string token = service.GenerateToken(curso);
    return new { token = token };
});

app.Run();


