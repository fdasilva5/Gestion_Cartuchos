namespace Models.DTOs;

public class Asignar_Impresora_DTO
{
    public int Id { get; set; }
    public required int impresora_id { get; set; }
    public required int cartucho_id { get; set; }
    public DateOnly fecha_asignacion { get; set; }
    public DateOnly? fecha_desasignacion { get; set; }
    public string? observaciones { get; set; }
}