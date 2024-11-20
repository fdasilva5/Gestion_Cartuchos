namespace Models.DTOs
{
    public class ImpresoraDTO
    {
        public int Id { get; set; }
        public required string Modelo { get; set; }
        public required string Marca { get; set; }
        public required int oficina_id { get; set; }
        public required Oficina oficina { get; set; }
        public ICollection<Modelo>? modelo_cartucho_compatible { get; set; }
    }
}