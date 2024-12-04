using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ModeloService : IModeloService
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
            var modelos = await _context.Modelos.ToListAsync();
            return _mapper.Map<IEnumerable<ModeloDTO>>(modelos);
        }

        public async Task<ModeloDTO> GetById(int id)
        {
            var modelo = await _context.Modelos.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ModeloDTO>(modelo);
        }

        public async Task<ModeloDTO> Create(ModeloDTO modeloDTO)
        {
            var modelo = _mapper.Map<Modelo>(modeloDTO);
            _context.Modelos.Add(modelo);
            await _context.SaveChangesAsync();
            return _mapper.Map<ModeloDTO>(modelo);
        }

        public async Task Update(int id, ModeloDTO modeloDTO)
        {
            var modelo = _mapper.Map<Modelo>(modeloDTO);
            modelo.Id = id;
            _context.Entry(modelo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var modelo = await _context.Modelos.FirstOrDefaultAsync(x => x.Id == id);
            _context.Modelos.Remove(modelo);
            await _context.SaveChangesAsync();
        }
    }
}