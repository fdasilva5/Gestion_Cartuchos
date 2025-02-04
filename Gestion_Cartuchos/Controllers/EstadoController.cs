using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using Models;
using Models.DTOs;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Controllers{
    
    [Route("api/[controller]")]
    [ApiController]

    public class EstadoController(IEstadoService estadoService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly IEstadoService _estadoService = estadoService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;

        [HttpGet]
        public async Task<IEnumerable<EstadoDTO>> GetAll()
        {
            return await _estadoService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<EstadoDTO> GetById(int id)
        {
            return await _estadoService.GetById(id);
        }

        [HttpPost]
        public async Task<Estado> Create(EstadoDTO estadoDTO)
        {
            var estado = await _estadoService.Create(estadoDTO);
            return _mapper.Map<Estado>(estado);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, EstadoDTO estadoDTO)
        {
            await _estadoService.Update(id, estadoDTO);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _estadoService.Delete(id);
        }
    }
}