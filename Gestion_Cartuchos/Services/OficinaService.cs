using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class OficinaService : IOficinaService
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public OficinaService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OficinaDTO>> GetAll()
        {
            var oficinas = await _context.Oficinas.ToListAsync();
            return _mapper.Map<IEnumerable<OficinaDTO>>(oficinas);
        }

        public async Task<OficinaDTO> GetById(int id)
        {
            var oficina = await _context.Oficinas.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<OficinaDTO>(oficina);
        }

        public async Task<OficinaDTO> Create(OficinaDTO oficinaDTO)
        {
            var oficina = _mapper.Map<Oficina>(oficinaDTO);
            _context.Oficinas.Add(oficina);
            await _context.SaveChangesAsync();
            return _mapper.Map<OficinaDTO>(oficina);
        }

        public async Task Update(int id, OficinaDTO oficinaDTO)
        {
            var oficina = _mapper.Map<Oficina>(oficinaDTO);
            oficina.Id = id;
            _context.Entry(oficina).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var oficina = await _context.Oficinas.FirstOrDefaultAsync(x => x.Id == id);
            _context.Oficinas.Remove(oficina);
            await _context.SaveChangesAsync();
        }
    }
}