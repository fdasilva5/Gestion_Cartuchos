using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Recargas
    {
        [Key]
        public int Id { get; set; }
        
        public required DateOnly fecha_recarga { get; set; }
        public required string observaciones { get; set; }
        
        public required Cartucho cartucho { get; set; }
        public required int cartucho_id { get; set; }
    }
}