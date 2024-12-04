using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class EstadoService : IEstadoService
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public EstadoService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EstadoDTO>> GetAll()
        {
            var estados = await _context.Estados.ToListAsync();
            return _mapper.Map<IEnumerable<EstadoDTO>>(estados);
        }

        public async Task<EstadoDTO> GetById(int id)
        {
            var estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<EstadoDTO>(estado);
        }

        public async Task<EstadoDTO> Create(EstadoDTO estadoDTO)
        {
            var estado = _mapper.Map<Estado>(estadoDTO);
            await _context.Estados.AddAsync(estado);
            await _context.SaveChangesAsync();
            return _mapper.Map<EstadoDTO>(estado);
        }

        public async Task Update(int id, EstadoDTO estadoDTO)
        {
            var estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == id);
            if (estado == null)
            {
                throw new InvalidOperationException("El estado no existe");
            }
            _mapper.Map(estadoDTO, estado);
            _context.Estados.Update(estado);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var estado = await _context.Estados.FirstOrDefaultAsync(x => x.Id == id);
            if (estado == null)
            {
                throw new InvalidOperationException("El estado no existe");
            }
            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();
        }
    }
}