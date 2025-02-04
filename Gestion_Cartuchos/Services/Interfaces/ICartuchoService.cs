using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICartuchoService
    {
        Task<IEnumerable<CartuchoDTO>> GetAll();
        Task<IEnumerable<CartuchoDTO>> GetByEstadoDisponible();
        Task<CartuchoDTO> GetById(int id);
        Task<CartuchoDTO> Create(CartuchoDTO cartuchoDTO);
        Task Update(int id, CartuchoDTO cartuchoDTO);
        Task ChangeEstadoToEnRecarga(int id);
        Task Delete(int id);
        Task<IEnumerable<ModeloDTO>> GetModelosCompatiblesConImpresora(int impresoraId);
    }
}