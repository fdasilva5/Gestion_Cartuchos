using Models.DTOs;

namespace Services
{
    public interface IRecargaService
    {
        Task<IEnumerable<RecargasDTO>> GetAll();
        Task<RecargasDTO> GetById(int id);
        Task<RecargasDTO> Create(RecargasDTO recargaDTO);
    }
}