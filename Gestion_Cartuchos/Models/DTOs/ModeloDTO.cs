namespace Models.DTOs
{
    public class ModeloDTO
    {
        public int Id { get; set; }
        public required string modelo_cartuchos { get; set; }
        public required string marca { get; set; }
        public required int stock { get; set; }
       
    }

}