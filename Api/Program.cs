using Microsoft.AspNetCore.Mvc;
using Api;
using Api.Endpoints;
using Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("escuela_db");

builder.Services.AddDbContext<EscuelaContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/api")
    .MapUsuarioEndpoints()
    .WithTags("Usuario");

app.MapGroup("/api")
    .MapRolEndpoints()
    .WithTags("Rol");
app.Run();