using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Cartucho
    {
        [Key]
        public int Id { get; set; }
        
        public required string numero_serie { get; set; }
        public required DateOnly fecha_alta { get; set; }
        public int cantidad_recargas { get; set; }
        public string? observaciones { get; set; }

        public required Modelo modelo { get; set; }
        public required int modelo_id { get; set; }
        public required int estado_id { get; set; }
        public required Estado estado { get; set; }
        
    }
}