using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Impresora
    {
        [Key]
        public int Id { get; set; }

        public required string modelo { get; set; }
        public required string marca { get; set; }  
        public required Oficina oficina { get; set; }
        public required int oficina_id { get; set; }
        public List<ImpresoraModelo>? ImpresoraModelos { get; set; } 
    }
}