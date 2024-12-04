using Models.DTOs;

namespace Services
{
    public interface IEstadoService
    {
        Task<IEnumerable<EstadoDTO>> GetAll();
        Task<EstadoDTO> GetById(int id);
        Task<EstadoDTO> Create(EstadoDTO estadoDTO);
        Task Update(int id, EstadoDTO estadoDTO);
        Task Delete(int id);
    }
}