using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Modelo
    {
        [Key]
        public int Id { get; set; }
        public required string modelo_cartuchos { get; set; }
        public required string marca { get; set; }
    
        public required int stock { get; set; } = 1;
        
        [JsonIgnore]
        public ICollection<Cartucho> Cartuchos { get; set; } = new List<Cartucho>();
    }
}