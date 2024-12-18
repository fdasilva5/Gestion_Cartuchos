using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class Asignar_ImpresoraService : IAsignar_Impresora_Service
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
                .ThenInclude(x => x.oficina)
                .Include(x => x.cartucho)
                .ThenInclude(x => x.modelo)
                .Where(x => x.fecha_desasignacion == null)
                .ToListAsync();
            return _mapper.Map<IEnumerable<Asignar_Impresora_DTO>>(asignar_impresoras);
        }

        public async Task<Asignar_Impresora_DTO> GetById(int id)
        {
            var asignar_impresora = await _context.Asignar_Impresoras
                .Include(x => x.impresora)
                .Include(x => x.cartucho)
                .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<Asignar_Impresora_DTO>(asignar_impresora);
        }

        public async Task<Asignar_Impresora> Create(Asignar_Impresora_DTO asignar_impresoraDTO)
        {
            if (asignar_impresoraDTO == null)
            {
                throw new ArgumentNullException(nameof(asignar_impresoraDTO));
            }

            var asignar_impresora = _mapper.Map<Asignar_Impresora>(asignar_impresoraDTO);

            var impresoraConCartuchoAsignado = await _context.Asignar_Impresoras
                .FirstOrDefaultAsync(x => x.impresora_id == asignar_impresoraDTO.impresora_id && x.fecha_desasignacion == null);

            if (impresoraConCartuchoAsignado != null)
            {
                throw new InvalidOperationException("Esta impresora ya tiene un cartucho asignado");
            }

            var cartucho_en_uso = await _context.Asignar_Impresoras
                .FirstOrDefaultAsync(x => x.cartucho.Id == asignar_impresoraDTO.cartucho_id && x.fecha_desasignacion == null);

            if (cartucho_en_uso != null)
            {
                throw new InvalidOperationException("Este cartucho ya está asignado a una impresora");
            }

            asignar_impresora.impresora = await _context.Impresoras.FirstOrDefaultAsync(x => x.Id == asignar_impresoraDTO.impresora_id);
            if (asignar_impresora.impresora == null)
            {
                throw new InvalidOperationException("La impresora especificada no existe");
            }

            var compatibilidad = await _context.ImpresoraModelos
                .FirstOrDefaultAsync(x => x.impresora_id == asignar_impresora.impresora_id && x.modelo_id == asignar_impresora.cartucho.modelo_id);

            if (compatibilidad == null)
            {
                throw new InvalidOperationException("El cartucho especificado no es compatible con la impresora");
            }

            asignar_impresora.cartucho = await _context.Cartuchos
                .Include(c => c.modelo)
                .FirstOrDefaultAsync(x => x.Id == asignar_impresoraDTO.cartucho_id);
            if (asignar_impresora.cartucho == null)
            {
                throw new InvalidOperationException("El cartucho especificado no existe");
            }

            if (asignar_impresora.cartucho.estado_id != 1)
            {
                throw new InvalidOperationException("El cartucho especificado no está disponible para asignación");
            }

            asignar_impresora.fecha_asignacion = DateOnly.FromDateTime(DateTime.Now);
            asignar_impresora.cartucho.estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == 2);
            if (asignar_impresora.cartucho.estado == null)
            {
                throw new InvalidOperationException("El estado especificado no existe");
            }

            asignar_impresora.cartucho.estado_id = 2;

            if (asignar_impresora.cartucho.modelo == null)
            {
                throw new InvalidOperationException("El modelo del cartucho especificado no existe");
            }

            asignar_impresora.cartucho.modelo.stock -= 1;

            _context.Asignar_Impresoras.Add(asignar_impresora);
            await _context.SaveChangesAsync();
            return asignar_impresora;
        }

        public async Task<Asignar_Impresora_DTO> Update(int id, Asignar_Impresora_DTO asignar_impresoraDTO)
        {
            var asignar_impresora = await _context.Asignar_Impresoras.FindAsync(id);
            if (asignar_impresora == null)
            {
                return null;
            }

            _mapper.Map(asignar_impresoraDTO, asignar_impresora);
            await _context.SaveChangesAsync();

            return _mapper.Map<Asignar_Impresora_DTO>(asignar_impresora);
        }

        public async Task<bool> Delete(int id)
        {
            var asignar_impresora = await _context.Asignar_Impresoras.FindAsync(id);
            if (asignar_impresora == null)
            {
                return false;
            }

            _context.Asignar_Impresoras.Remove(asignar_impresora);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DesasignarCartucho(int cartuchoId)
        {
            var asignaciones = await _context.Asignar_Impresoras
                .Include(x => x.cartucho)
                .Where(x => x.cartucho_id == cartuchoId && x.fecha_desasignacion == null)
                .ToListAsync();

            if (!asignaciones.Any())
            {
                throw new InvalidOperationException("No se encontró ninguna asignación activa del cartucho.");
            }

            var estadoDesasignado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == 3);
            if (estadoDesasignado == null)
            {
                throw new InvalidOperationException("El estado especificado no existe.");
            }

            foreach (var asignar_impresora in asignaciones)
            {
                asignar_impresora.cartucho.estado_id = 3;
                asignar_impresora.cartucho.estado = estadoDesasignado;
                asignar_impresora.fecha_desasignacion = DateOnly.FromDateTime(DateTime.Now);
            }

            await _context.SaveChangesAsync();
        }
    }
}