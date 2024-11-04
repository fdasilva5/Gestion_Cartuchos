using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RecargaController : ControllerBase
    {
        private readonly RecargaService _recargaService;
        private readonly IMapper _mapper;

        public RecargaController(RecargaService recargaService, IMapper mapper)
        {
            _recargaService = recargaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<RecargasDTO>> GetAll()
        {
            return await _recargaService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<RecargasDTO> GetById(int id)
        {
            return await _recargaService.GetById(id);
        }

        [HttpPost]
        public async Task<Recargas> Create(RecargasDTO recargaDTO)
        {
            return await _recargaService.Create(recargaDTO);
        }

        
    }
}