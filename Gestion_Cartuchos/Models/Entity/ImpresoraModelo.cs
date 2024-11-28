using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Models;

public class ImpresoraModelo
    {
        [Key]
        public int Id { get; set; }
        public required int impresora_id { get; set; }
        public required int modelo_id { get; set; }

        [JsonIgnore]
        public required Impresora impresora { get; set; }
        public required Modelo modelo { get; set; }
    
    }