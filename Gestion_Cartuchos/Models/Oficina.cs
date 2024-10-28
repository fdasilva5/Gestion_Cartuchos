using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Oficina
    {
        [Key]
        public int Id { get; set; }
        public required string nombre { get; set; }
    }
}