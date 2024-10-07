
namespace Api;

public class Usuario
{
    public int IdUsuario { get; set; }
    public required string Nombre { get; set; }
    public required string Email {get ; set;}
    public required string NombreUsuario {get ; set;}
    public required string Contraseñia {get ; set;}
    public bool Habilitado {get; set;}
    public DateTime FechaCreacion {get; set;}
    public List<Rol> Roles = new List<Rol>();

}