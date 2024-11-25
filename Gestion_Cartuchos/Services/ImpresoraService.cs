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

        public async Task<IEnumerable<ModeloDTO>> GetCompatibleModelos(int impresoraId)
        {
            var impresoraModelos = await _context.ImpresoraModelos
                .Include(im => im.modelo)
                .Where(im => im.impresora_id == impresoraId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ModeloDTO>>(impresoraModelos.Select(im => im.modelo));
        }

        public async Task<IEnumerable<ImpresoraDTO>> GetAll()
        {
            var impresoras = await _context.Impresoras
            .Include(x => x.ImpresoraModelos)
            .Include(x => x.oficina)
            .ToListAsync();
            return _mapper.Map<IEnumerable<ImpresoraDTO>>(impresoras);
        }

        

        public async Task<ImpresoraDTO> GetById(int id)
        {
            var impresora = await _context.Impresoras
            .Include(x => x.oficina)
            .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ImpresoraDTO>(impresora);
        }



        public async Task<ImpresoraDTO> Create(ImpresoraDTO impresoraDTO)
        {
            var impresora = _mapper.Map<Impresora>(impresoraDTO);
            var oficina = await _context.Oficinas.FindAsync(impresoraDTO.oficina_id);

            if (oficina == null)
            {
                throw new Exception("Oficina no encontrada");
            }

            impresora.oficina = oficina;

            
            await _context.Impresoras.AddAsync(impresora);
            await _context.SaveChangesAsync();

            foreach (var modeloId in impresoraDTO.CompatibleModeloIds)
            {
                await CreateImpresoraModelo(impresora.Id, modeloId);
            }
            
            return _mapper.Map<ImpresoraDTO>(impresora);
        }



        public async Task<ImpresoraModelo> CreateImpresoraModelo(int impresoraId, int modeloId)
        {
            var impresora = await _context.Impresoras.FindAsync(impresoraId);
            var modelo = await _context.Modelos.FindAsync(modeloId);

            if (impresora == null || modelo == null)
            {
                throw new Exception("Impresora o Modelo no encontrado");
            }

            var impresoraModelo = new ImpresoraModelo
            {
                impresora_id = impresoraId,
                modelo_id = modeloId,
                impresora = impresora,
                modelo = modelo
            };

            await _context.ImpresoraModelos.AddAsync(impresoraModelo);
            await _context.SaveChangesAsync();

            return impresoraModelo;
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