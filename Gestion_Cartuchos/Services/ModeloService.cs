using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ModeloService
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public ModeloService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloDTO>> GetAll()
        {
            var modelos = await _context.Modelos
            .ToListAsync();
            return _mapper.Map<IEnumerable<ModeloDTO>>(modelos);
        }

        public async Task<ModeloDTO> GetById(int id)
        {
            var Modelo = await _context.Modelos
            .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ModeloDTO>(Modelo);
        }

        public async Task<ModeloDTO> Create(ModeloDTO ModeloDTO)
        {
            var Modelo = _mapper.Map<Modelo>(ModeloDTO);
            _context.Modelos.Add(Modelo);
            await _context.SaveChangesAsync();
            return _mapper.Map<ModeloDTO>(Modelo);
        }

        public async Task Update(int id, ModeloDTO ModeloDTO)
        {
            var Modelo = _mapper.Map<Modelo>(ModeloDTO);
            Modelo.Id = id;
            _context.Entry(Modelo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var Modelo = await _context.Modelos.FirstOrDefaultAsync(x => x.Id == id);
            _context.Modelos.Remove(Modelo);
            await _context.SaveChangesAsync();
        }

        
        
    }
}