using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IOficinaService
    {
        Task<IEnumerable<OficinaDTO>> GetAll();
        Task<OficinaDTO> GetById(int id);
        Task<OficinaDTO> Create(OficinaDTO oficinaDTO);
        Task Update(int id, OficinaDTO oficinaDTO);
        Task Delete(int id);
    }
}