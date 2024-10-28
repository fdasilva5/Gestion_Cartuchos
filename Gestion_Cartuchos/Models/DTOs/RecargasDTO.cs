namespace Models.DTOs
{
    public class RecargasDTO
    {
        public int Id { get; set; }
        public DateOnly fecha_recarga { get; set; }
        public string? observaciones { get; set; }
        public int cartucho_id { get; set; }
        public required CartuchoDTO cartucho { get; set; }
    }
}