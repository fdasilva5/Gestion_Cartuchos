using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RecargaController : ControllerBase
    {
        private readonly IRecargaService _recargaService;
        private readonly IMapper _mapper;

        public RecargaController(IRecargaService recargaService, IMapper mapper)
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
        public async Task<RecargasDTO> Create(RecargasDTO recargaDTO)
        {
            var recarga = await _recargaService.Create(recargaDTO);
            return _mapper.Map<RecargasDTO>(recarga);
        }

        
    }
}