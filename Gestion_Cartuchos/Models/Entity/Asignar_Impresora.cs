namespace Models
{
    public class Asignar_Impresora
    {
        public int Id { get; set; }

        public int oficina_id { get; set; }
        public int impresora_id { get; set; }
        public required Impresora impresora { get; set; }
        public int cartucho_id { get; set; }
        public required Cartucho cartucho { get; set; }
        public DateOnly fecha_asignacion { get; set; }
        public DateOnly? fecha_desasignacion { get; set; } 
    }
}