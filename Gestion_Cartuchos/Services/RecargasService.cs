using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class RecargaService
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public RecargaService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecargasDTO>> GetAll()
        {
            var recargas = await _context.Recargas
            .Include(x => x.cartucho)
            .ThenInclude(x => x.estado)
            .Include(x => x.cartucho)
            .ThenInclude(x => x.modelo)
            .ToListAsync();
            return _mapper.Map<IEnumerable<RecargasDTO>>(recargas);
        }

        public async Task<RecargasDTO> GetById(int id)
        {
            var recarga = await _context.Recargas
            .Include(x => x.cartucho)
            .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<RecargasDTO>(recarga);
        }


        public async Task<Recargas> Create(RecargasDTO recargaDTO)
        {
            var recarga = _mapper.Map<Recargas>(recargaDTO);

            recarga.cartucho = await _context.Cartuchos.FirstOrDefaultAsync(x => x.Id == recargaDTO.cartucho_id);
            recarga.fecha_recarga = DateOnly.FromDateTime(DateTime.Now);
            
            if (recarga.cartucho.cantidad_recargas == 4)
            {
                throw new InvalidOperationException("El cartucho ya alcanzo el maximo de recargas permitidas");
            }

            recarga.cartucho.cantidad_recargas += 1; //Cuando llega a 4 tiene que generar una alerta
            
            if (recarga.cartucho.cantidad_recargas == 4)
            {
                //Generar alerta
            }

            recarga.cartucho.modelo.stock += 1;
            
            _context.Recargas.Add(recarga);
            await _context.SaveChangesAsync();
            return recarga;
        }




    }
}