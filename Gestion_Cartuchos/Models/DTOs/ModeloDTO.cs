namespace Models.DTOs
{
    public class ModeloDTO
    {
        public required string modelo { get; set; }
        public required string marca { get; set; }

    public int stock { get; set; } = 1;
    }
}