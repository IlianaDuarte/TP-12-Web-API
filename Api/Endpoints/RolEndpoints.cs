using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;
using Models;

public static class RolEndpoints
{
    public static RouteGroupBuilder MapRolEndpoints(this RouteGroupBuilder app)
    {
        /*    List<Rol> roles = [
        new Rol {Id = Guid.NewGuid(), Nombre = "Rector", Habilitado = true, Fechacreacion = DateTime.Now},
        new Rol {Id = Guid.NewGuid(), Nombre = "Preceptor", Habilitado = true, Fechacreacion = DateTime.Now},
        new Rol {Id = Guid.NewGuid(), Nombre = "Profesor", Habilitado = true, Fechacreacion = DateTime.Now}

    ];*/


        app.MapPost("/rol", ([FromBody] Rol rol, EscuelaContext context) =>
        {
            if (string.IsNullOrWhiteSpace(rol.Nombre))
            {
                return Results.BadRequest(); // Código 400
            }

            rol.Fechacreacion = DateTime.Now;
            rol.Habilitado = true;
            context.Rols.Add(rol);
            return Results.Created($"/usuario/{rol.Id}", rol);
        })
            .WithTags("Rol");


        //Lee listado de rol
        app.MapGet("/roles", (EscuelaContext context) =>
        {
            return Results.Ok(context.Rols);
        })
            .WithTags("Rol");

        //Lee por Id
        app.MapGet("/rol/{IdRol}", (Guid IdRol, EscuelaContext context) =>
        {
            var rolPorId = context.Rols.FirstOrDefault(rol => rol.Id == IdRol);
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
        app.MapPut("/rol", ([FromQuery] Guid IdRol, [FromBody] Rol rol, EscuelaContext context) =>
        {
            var rolActualizar = context.Rols.FirstOrDefault(rol => rol.Id == IdRol);
            if (rolActualizar != null)
            {
                rolActualizar.Nombre = rol.Nombre;
                return Results.Ok(context.Rols); //Codigo 200
            }
            else
            {
                return Results.NotFound(); //Codigo 404
            }
        })
            .WithTags("Rol");

        //Elimina roles
        app.MapDelete("/rol", ([FromQuery] Guid IdRol, EscuelaContext context) =>
        {
            var rolEliminar = context.Rols.FirstOrDefault(Rol => Rol.Id == IdRol);
            if (rolEliminar != null)
            {
                context.Rols.Remove(rolEliminar);
                return Results.NoContent(); //Codigo 204
            }
            else
            {
                return Results.NotFound(); //Codigo 404
            }
        })
            .WithTags("Rol");

        // cruzados
        app.MapPost("/rol/{IdRol}/usuario/{IdUsuario}", (Guid IdRol, int IdUsuario, EscuelaContext context) =>
    {
        var rol = context.Rols.FirstOrDefault(rol => rol.Id == IdRol);
        var usuario = context.Usuarios.FirstOrDefault(usuario => usuario.Id == IdUsuario);

        if (usuario != null && rol != null)
        {
            //alumno.Cursos.Add(curso);
            context.Usuariorols.Add(new Usuariorol { Idrol = IdRol, Idusuario = IdUsuario });
            return Results.Ok();
        }

        return Results.NotFound();
    })
        .WithTags("Rol");

        app.MapDelete("/rol/{IdRol}/usuario/{IdUsuario}", (Guid IdRol, int IdUsuario, EscuelaContext context) =>
        {
            var usuariorol = context.Usuariorols.FirstOrDefault(usuariorol => usuariorol.Idrol == IdRol && usuariorol.Idusuario == IdUsuario);

            if (usuariorol != null && usuariorol != null)
            {
                // Eliminar el usuario del rol
                context.Usuariorols.Remove(usuariorol);
                return Results.Ok();
            }

            return Results.NotFound();
        })
            .WithTags("Rol");


        return app;
    }
}
