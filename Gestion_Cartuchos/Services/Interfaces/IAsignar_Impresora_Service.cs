using Models.DTOs;
using Models;

namespace Services
{
    public interface IAsignar_Impresora_Service
    {
        Task<Asignar_Impresora> Create(Asignar_Impresora_DTO asignarImpresoraDTO);
        Task<Asignar_Impresora_DTO> Update(int id, Asignar_Impresora_DTO asignarImpresoraDTO);
        Task<bool> Delete(int id);
        Task<Asignar_Impresora_DTO> GetById(int id);
        Task<IEnumerable<Asignar_Impresora_DTO>> GetAll();
        Task DesasignarCartucho(int cartuchoId);
    }
}