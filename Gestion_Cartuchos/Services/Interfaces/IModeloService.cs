using Models.DTOs;

namespace Services
{
    public interface IModeloService
    {
        Task<IEnumerable<ModeloDTO>> GetAll();
        Task<ModeloDTO> GetById(int id);
        Task<ModeloDTO> Create(ModeloDTO modeloDTO);
        Task Update(int id, ModeloDTO modeloDTO);
        Task Delete(int id);
    }
}