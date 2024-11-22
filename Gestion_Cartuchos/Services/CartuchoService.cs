using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class CartuchoService 
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public CartuchoService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartuchoDTO>> GetAll()
        {
            var cartuchos = await _context.Cartuchos
            .Include(x => x.modelo)
            .Include(x => x.estado)
            .ToListAsync();
            return _mapper.Map<IEnumerable<CartuchoDTO>>(cartuchos);
        }

        public async Task<IEnumerable<CartuchoDTO>> GetByEstadoDisponible()
        {
            var cartuchos = await _context.Cartuchos
                .Include(x => x.modelo)
                .Include(x => x.estado)
                .Where(x => x.estado_id == 1)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CartuchoDTO>>(cartuchos);
        }

        public async Task<IEnumerable<CartuchoDTO>> GetByImpresoraId(int impresoraId)
        {
            var impresora = await _context.Impresoras
            .Include(i => i.modelos_cartucho_compatibles)
            .FirstOrDefaultAsync(i => i.Id == impresoraId);

            if (impresora == null)
            {
            throw new Exception("Impresora no encontrada");
            }

            var modelosCompatiblesIds = impresora.modelos_cartucho_compatibles
            .Select(m => m.Id)
            .ToList();

            var cartuchosCompatibles = await _context.Cartuchos
            .Where(c => modelosCompatiblesIds.Contains(c.modelo_id) && c.estado_id == 1)
            .Include(c => c.modelo)
            .ToListAsync();

            return _mapper.Map<IEnumerable<CartuchoDTO>>(cartuchosCompatibles);
        }

        public async Task<CartuchoDTO> GetById(int id)
        {
            var cartucho = await _context.Cartuchos
            .Include(x => x.modelo)
            .Include(x => x.estado)
            .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<CartuchoDTO>(cartucho);
        }

        public async Task<Cartucho> Create(CartuchoDTO cartuchoDTO)
        {
            var cartucho = _mapper.Map<Cartucho>(cartuchoDTO);
            
            cartucho.modelo = await _context.Modelos.FirstOrDefaultAsync(x => x.Id == cartuchoDTO.modelo_id);
            cartucho.estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == 1);
            cartucho.estado_id = 1; // Estado: Disponible
            cartucho.fecha_alta = DateOnly.FromDateTime(DateTime.Now);
            cartucho.modelo.stock += 1;
            _context.Cartuchos.Add(cartucho);
            await _context.SaveChangesAsync();
            return cartucho;
        }

        public async Task Update(int id, CartuchoDTO cartuchoDTO)
        {
            var cartucho = _mapper.Map<Cartucho>(cartuchoDTO);
            cartucho.Id = id;
            _context.Entry(cartucho).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeEstadoToEnRecarga(int id)
        {
            var cartucho = await _context.Cartuchos.FirstOrDefaultAsync(x => x.Id == id);
            var estadoCartucho = await _context.Estados.FirstOrDefaultAsync(x => x.Id == 5);
            
            if (cartucho != null)
            {
                cartucho.estado_id = 5;
                cartucho.estado = estadoCartucho;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var cartucho = await _context.Cartuchos.FirstOrDefaultAsync(x => x.Id == id);
            _context.Cartuchos.Remove(cartucho);
            await _context.SaveChangesAsync();
        }
    }

    
}