using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Modelo
    {
        [Key]
        public int Id { get; set; }
        public required string modelo_cartuchos { get; set; }
        public required string marca { get; set; }

        public required int stock { get; set; } = 1;
    }
}