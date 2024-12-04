using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloCOntroller(IModeloService modeloService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly IModeloService _modeloService = modeloService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;

        [HttpGet]
        public async Task<IEnumerable<ModeloDTO>> GetAll()
        {
            return await _modeloService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ModeloDTO> GetById(int id)
        {
            var modelo = await _modeloService.GetById(id);
            return _mapper.Map<ModeloDTO>(modelo);
        }

        [HttpPost]
        public async Task<Modelo> Create(ModeloDTO modeloDTO)
        {
            var modelo = await _modeloService.Create(modeloDTO);
            return _mapper.Map<Modelo>(modelo);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, ModeloDTO modeloDTO)
        {
            await _modeloService.Update(id, modeloDTO);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _modeloService.Delete(id);
        }
    } 
    
}
