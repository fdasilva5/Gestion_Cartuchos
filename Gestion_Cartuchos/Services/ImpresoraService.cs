using AutoMapper;
using Models;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ImpresoraService
    {
        private readonly Gestion_Cartuchos_Context _context;
        private readonly IMapper _mapper;

        public ImpresoraService(Gestion_Cartuchos_Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ImpresoraDTO>> GetAll()
        {
            var impresoras = await _context.Impresoras
            .ToListAsync();
            return _mapper.Map<IEnumerable<ImpresoraDTO>>(impresoras);
        }

        public async Task<ImpresoraDTO> GetById(int id)
        {
            var impresora = await _context.Impresoras
            .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ImpresoraDTO>(impresora);
        }

        public async Task<ImpresoraDTO> Create(ImpresoraDTO impresoraDTO)
        {
            var impresora = _mapper.Map<Impresora>(impresoraDTO);
            _context.Impresoras.Add(impresora);
            await _context.SaveChangesAsync();
            return _mapper.Map<ImpresoraDTO>(impresora);
        }

        public async Task Update(int id, ImpresoraDTO impresoraDTO)
        {
            var impresora = _mapper.Map<Impresora>(impresoraDTO);
            impresora.Id = id;
            _context.Entry(impresora).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var impresora = await _context.Impresoras.FirstOrDefaultAsync(x => x.Id == id);
            _context.Impresoras.Remove(impresora);
            await _context.SaveChangesAsync();
        }



    }
}