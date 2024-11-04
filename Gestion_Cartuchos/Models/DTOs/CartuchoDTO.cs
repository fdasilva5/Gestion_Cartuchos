namespace Models.DTOs;

public class CartuchoDTO
{
    public int Id { get; set; }
    public required string numero_serie { get; set; }
    public DateOnly fecha_alta { get; set; }
    public int cantidad_recargas { get; set; }
    public string? observaciones { get; set; }
    public int modelo_id { get; set; }
    public required Modelo modelo { get; set; }
    
    public int estado_id { get; set; }
    public required Estado estado { get; set; }

}