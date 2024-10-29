using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class Asignar_ImpresoraService
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public Asignar_ImpresoraService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Asignar_Impresora_DTO>> GetAll()
        {
            var asignar_impresoras = await _context.Asignar_Impresoras
            .Include(x => x.impresora)
            .Include(x => x.cartucho)
            .ToListAsync();
            return _mapper.Map<IEnumerable<Asignar_Impresora_DTO>>(asignar_impresoras);
        }

        public async Task<Asignar_Impresora_DTO> GetById(int id)
        {
            var asignar_impresora = await _context.Asignar_Impresoras.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<Asignar_Impresora_DTO>(asignar_impresora);
        }

        public async Task<Asignar_Impresora> Create(Asignar_Impresora_DTO asignar_impresoraDTO)
        {
            var asignar_impresora = _mapper.Map<Asignar_Impresora>(asignar_impresoraDTO);

            
            var imporesora_con_cartucho_asignado = await _context.Asignar_Impresoras
                .FirstOrDefaultAsync(x => x.impresora.Id == asignar_impresoraDTO.impresora_id);
            if (imporesora_con_cartucho_asignado != null)
            {
                throw new InvalidOperationException("Esta impresora ya tiene un cartucho asignado");
            }
            var cartucho_en_uso = await _context.Asignar_Impresoras
                .FirstOrDefaultAsync(x => x.cartucho.Id == asignar_impresoraDTO.cartucho_id);
            if (cartucho_en_uso != null)
            {
                throw new InvalidOperationException("Este cartucho ya esta asignado a una impresora");
            }
            
            asignar_impresora.impresora = await _context.Impresoras.FirstOrDefaultAsync(x => x.Id == asignar_impresoraDTO.impresora_id);
            asignar_impresora.oficina = await _context.Oficinas.FirstOrDefaultAsync(x => x.Id == asignar_impresoraDTO.oficina_id);
            asignar_impresora.cartucho = await _context.Cartuchos.FirstOrDefaultAsync(x => x.Id == asignar_impresoraDTO.cartucho_id);
            asignar_impresora.fecha_asignacion = DateOnly.FromDateTime(DateTime.Now);
            asignar_impresora.cartucho.estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == 2); 
            asignar_impresora.cartucho.estado_id = 2;
            _context.Asignar_Impresoras.Add(asignar_impresora);
            await _context.SaveChangesAsync();
            return asignar_impresora;
        }

        public async Task Update(int id, Asignar_Impresora_DTO asignar_impresoraDTO)
        {
            var asignar_impresora = _mapper.Map<Asignar_Impresora>(asignar_impresoraDTO);
            asignar_impresora.Id = id;
            _context.Entry(asignar_impresora).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var asignar_impresora = await _context.Asignar_Impresoras.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DesasignarCartucho(int idCartucho)
        {
            var asignar_impresora = await _context.Asignar_Impresoras
                .Include(x => x.cartucho)
                .FirstOrDefaultAsync(x => x.cartucho_id == idCartucho);

            if (asignar_impresora == null)
            {
                throw new InvalidOperationException("No se encontró la asignación del cartucho.");
            }

            asignar_impresora.cartucho.estado_id = 3;
            asignar_impresora.cartucho.estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == 3);
            asignar_impresora.fecha_desasignacion = DateOnly.FromDateTime(DateTime.Now);
            
            await _context.SaveChangesAsync();
        }
        
    }
}