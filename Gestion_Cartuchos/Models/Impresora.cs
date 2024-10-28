using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Impresora
    {
        [Key]
        public int Id { get; set; }

        public required string modelo { get; set; }
        public required string marca { get; set; }  
        
    }
}