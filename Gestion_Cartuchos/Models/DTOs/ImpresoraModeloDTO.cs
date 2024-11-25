using Models.DTOs;

public class ImpresoraModeloDTO
    {
        public int Id { get; set; }
        public required int modelo_id { get; set; }
        public required int impresora_id { get; set; }
    }