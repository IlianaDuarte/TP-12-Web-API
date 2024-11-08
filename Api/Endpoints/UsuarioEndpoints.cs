using Microsoft.AspNetCore.Mvc;
namespace Api.Endpoints;
using Models;

public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(this RouteGroupBuilder app)
    {
    /*    List<Usuario> Usuarios = [
    new Usuario {Id = 1, Nombre = "Mari", Email = "mariangel@gmail.com", Nombreusuario = "Mariangel", Contrasenia = "tay1989", Habilitado = true, Fechacreacion = DateTime.Now},
    new Usuario {Id = 2, Nombre = "Vani", Email = "vanina@gmail.com", Nombreusuario = "VaniBlas", Contrasenia = "gatitos", Habilitado = true, Fechacreacion = DateTime.Now},
    new Usuario {Id = 3, Nombre = "Ili", Email = "iliduarte@gmail.com", Nombreusuario = "IliDuarte", Contrasenia = "floricienta", Habilitado = true, Fechacreacion = DateTime.Now}
]; */

    app.MapPost("/usuario", ([FromBody] Usuario usuario, EscuelaContext context) => {
    // Validar si alguno de los campos del usuario es vacío o null
    if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
        string.IsNullOrWhiteSpace(usuario.Email) ||
        string.IsNullOrWhiteSpace(usuario.Nombreusuario) ||
        string.IsNullOrWhiteSpace(usuario.Contrasenia))
    {
        return Results.BadRequest();
    }
    usuario.Fechacreacion = DateTime.Now;
    usuario.Habilitado = true;
    context.Usuarios.Add(usuario);
    return Results.Created($"/usuario/{usuario.Id}", usuario);
});

    app.MapGet("/usuarios", (EscuelaContext context) =>
{
    return Results.Ok(context.Usuarios);
})
    .WithTags("Usuario");

app.MapGet("/usuario/{IdUsuario}", (int IdUsuario, EscuelaContext context) =>
{
    var usuarioPorId = context.Usuarios.FirstOrDefault(usuario => usuario.Id == IdUsuario);
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
app.MapPut("/usuario/{IdUsuario}", ([FromQuery] int IdUsuario, [FromBody] Usuario usuario, EscuelaContext context) =>
{
    var usuarioActualizar = context.Usuarios.FirstOrDefault(usuario => usuario.Id == IdUsuario);
    
    if (usuarioActualizar == null)
    {
        return Results.NotFound(); // 404
    }
    if (usuario.Nombre == null){

        return Results.BadRequest(); //400 Bad Request
    }

    usuarioActualizar.Email = usuario.Email;
    usuarioActualizar.Nombreusuario = usuario.Nombreusuario;
    usuarioActualizar.Contrasenia = usuario.Contrasenia;

    return Results.NoContent(); // 204
})
    .WithTags("Usuario");

//Elimina usuarios
app.MapDelete("/usuario", ([FromQuery] int IdUsuario, EscuelaContext context) =>
{
    var usuarioEliminar = context.Usuarios.FirstOrDefault(usuario => usuario.Id == IdUsuario);
    if (usuarioEliminar != null)
    {
        context.Usuarios.Remove(usuarioEliminar);
        return Results.NoContent(); //Codigo 204
    }
    else
    {
        return Results.NotFound(); //Codigo 404
    }
})
    .WithTags("Usuario");

    //cruzados
    app.MapPost("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario, Guid IdRol, EscuelaContext context) =>
{
    var usuario = context.Usuarios.FirstOrDefault(usuario => usuario.Id == IdUsuario);
    var rol = context.Rols.FirstOrDefault(rol => rol.Id == IdRol);

    if (usuario != null && rol != null)
    {
        // Agregar el rol al usuario
       context.Usuariorols.Add(new Usuariorol { Idrol = IdRol, Idusuario = IdUsuario });
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");

app.MapDelete("/usuario/{IdUsuario}/rol/{IdRol}", (int IdUsuario,  Guid IdRol, EscuelaContext context) =>
{
    var usuariorol = context.Usuariorols.FirstOrDefault(usuariorol => usuariorol.Idrol == IdRol && usuariorol.Idusuario == IdUsuario);

    if (usuariorol != null && usuariorol != null)
    {
        // Eliminar el rol del usuario
        context.Usuariorols.Remove(usuariorol);
        return Results.Ok();
    }

    return Results.NotFound();
})
    .WithTags("Usuario");

    return app;
    }
}