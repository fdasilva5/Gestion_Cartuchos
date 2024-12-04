using Models.DTOs;

namespace Services
{
    public interface IImpresoraService
    {
        Task<IEnumerable<ModeloDTO>> GetCompatibleModelos(int impresoraId);
        Task<IEnumerable<ImpresoraDTO>> GetAll();
        Task<ImpresoraDTO> GetById(int id);
        Task<ImpresoraDTO> Create(ImpresoraDTO impresoraDTO);
        Task<ImpresoraModelo> CreateImpresoraModelo(int impresoraId, int modeloId);
        Task Update(int id, ImpresoraDTO impresoraDTO);
        Task Delete(int id);
    }
}