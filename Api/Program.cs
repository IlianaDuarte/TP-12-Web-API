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

List<Rol> roles = [
    new Rol {IdRol = 1, Nombre = "Mariangel", Habilitado = true, FechaCreacion = DateTime.Now},
    new Rol {IdRol = 2, Nombre = "Vanina", Habilitado = true, FechaCreacion = DateTime.Now}
];

//crea un nuevo alumno en la lista
app.MapPost("/usuario", ([FromBody] Usuario usuario) =>
{
    if(string.IsNullOrWhiteSpace(usuario.Nombre)|| 
        string.IsNullOrWhiteSpace(usuario.Email) ||
        string.IsNullOrWhiteSpace(usuario.NombreUsuario) ||
        string.IsNullOrWhiteSpace(usuario.Contraseñia))
    {
        return Results.BadRequest();
    }
    usuario.FechaCreacion = DateTime.Now;
    usuario.Habilitado = true;
    Usuarios.Add(usuario);
    return Results.Created($"/usuario/{usuario.IdUsuario}", usuario);
});

//Lee listado de usuarios
app.MapGet("/usuario", () =>
{
    return Results.Ok(Usuarios);
})
    .WithTags("Usuario");

app.MapGet("/usuario/{IdUsuario}", (int IdUsuario) =>
{
    return Results.Ok(Usuarios);
})
    .WithTags("Usuario");


app.Run();

