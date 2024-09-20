using Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Endpoins
//Test
List<Usuario> Usuarios = [
    new Usuario {IdUsuario = 1, Nombre = "Mari", Email = "mariangel@gmail.com", NombreUsuario = "Mariangel", Contraseñia = "tay1989", Habilitado = true, FechaCreacion = DateTime.Now},
    new Usuario {IdUsuario = 2, Nombre = "Vani", Email = "vanina@gmail.com", NombreUsuario = "VaniBlas", Contraseñia = "gatitos", Habilitado = true, FechaCreacion = DateTime.Now},
    new Usuario {IdUsuario = 3, Nombre = "Ili", Email = "iliduarte@gmail.com", NombreUsuario = "IliDuarte", Contraseñia = "floricienta", Habilitado = true, FechaCreacion = DateTime.Now}
];

//crea un nuevo alumno en la lista
app.MapPost("/usuario", ([FromBody] Usuario usuario) =>
{
    Usuarios.Add(usuario);
    return Results.Ok(Usuarios);
});

app.Run();

