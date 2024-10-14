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
    new Rol {IdRol = 1, Nombre = "Rector", Habilitado = true, FechaCreacion = DateTime.Now},
    new Rol {IdRol = 2, Nombre = "Preceptor", Habilitado = true, FechaCreacion = DateTime.Now},
    new Rol {IdRol = 3, Nombre = "Profesor", Habilitado = true, FechaCreacion = DateTime.Now}
];

//crea un nuevo alumno en la lista
app.MapPost("/usuario", ([FromBody] Usuario usuario) => {
    // Validar si alguno de los campos del usuario es vacío o null
    if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
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
app.MapGet("/usuarios", () =>
{
    return Results.Ok(Usuarios);
})
    .WithTags("Usuario");

app.MapGet("/usuario/{IdUsuario}", (int IdUsuario) =>
{
    var usuarioPorId = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    if (usuarioPorId != null)
    {
        return Results.Ok(usuarioPorId); //Codigo 200
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Usuario");

//Actualiza usaurios
app.MapPut("/usuario", ([FromQuery] int IdUsuario, [FromBody] Usuario usuario) =>
{
    var usuarioActualizar = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    
    if (usuarioActualizar == null)
    {
        return Results.NotFound(); // 404
    }
    if (usuario.Nombre == null){

        return Results.BadRequest(); //400 Bad Request
    }

    usuarioActualizar.Email = usuario.Email;
    usuarioActualizar.NombreUsuario = usuario.NombreUsuario;
    usuarioActualizar.Contraseñia = usuario.Contraseñia;

    return Results.NoContent(); // 204
})
    .WithTags("Usuario");

//Elimina usuarios
app.MapDelete("/usuario", ([FromQuery] int IdUsuario) =>
{
    var usuarioEliminar = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    if (usuarioEliminar != null)
    {
        Usuarios.Remove(usuarioEliminar);
        return Results.NoContent(); //Codigo 204
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Usuario");


//Crea un rol
app.MapPost("/rol", ([FromBody] Rol rol) =>
{
    if(string.IsNullOrWhiteSpace(rol.Nombre))
    {
        return Results.BadRequest(); // Código 400
    }

    rol.FechaCreacion = DateTime.Now;
    rol.Habilitado = true;
    roles.Add(rol);
    return Results.Created($"/usuario/{rol.IdRol}", rol);
})
    .WithTags("Rol");


//Lee listado de rol
app.MapGet("/roles", () =>
{
    return Results.Ok(roles);
})
    .WithTags("Rol");

//Lee por Id
app.MapGet("/rol/{IdRol}", (int IdRol) =>
{
    var rolPorId = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    if (rolPorId != null)
    {
        return Results.Ok(rolPorId); //Codigo 200
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Rol");

//Actualiza roles
app.MapPut("/rol", ([FromQuery] int IdRol, [FromBody] Rol rol) =>
{
    var rolActualizar = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    if (rolActualizar != null)
    {
        rolActualizar.Nombre = rol.Nombre;
        return Results.Ok(roles); //Codigo 200
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Rol");

//Elimina roles
app.MapDelete("/rol", ([FromQuery] int IdRol) =>
{
    var rolEliminar = roles.FirstOrDefault(Rol => Rol.IdRol == IdRol);
    if (rolEliminar != null)
    {
        roles.Remove(rolEliminar);
        return Results.NoContent(); //Codigo 204
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Rol");



app.MapPost("/rol/{IdRol}/usuario/{IdUsuario}", (int IdRol, int IdUsuario) =>
{
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    var usuario = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);

    if (usuario != null && rol != null)
    {
        //alumno.Cursos.Add(curso);
        rol.Usuarios.Add(usuario);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Rol");

app.MapDelete("/rol/{IdRol}/usuario/{IdUsuario}", (int IdRol, int IdUsuario) =>
{
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);
    var usuario = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);

    if (usuario != null && rol != null)
    {
        // Eliminar el usuario del rol
        rol.Usuarios.Remove(usuario);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Rol");

app.MapPost("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, int IdRol) =>
{
    var usuario = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);

    if (usuario != null && rol != null)
    {
        // Agregar el rol al usuario
        usuario.Roles.Add(rol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");

app.MapDelete("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, int IdRol) =>
{
    var usuario = Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);
    var rol = roles.FirstOrDefault(rol => rol.IdRol == IdRol);

    if (usuario != null && rol != null)
    {
        // Eliminar el rol del usuario
        usuario.Roles.Remove(rol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");

app.Run();