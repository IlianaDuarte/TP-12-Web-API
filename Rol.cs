namespace Api;

public class Rol
{
    public int Id { get; set; }
    public required string Nombre {get; set;}
    public bool Habilitado {get; set;}
    public DateTime FechaCreacion {get; set;}
}