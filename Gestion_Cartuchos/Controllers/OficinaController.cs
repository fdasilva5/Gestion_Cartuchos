using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using Models;
using Models.DTOs;
using Services;

namespace Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class OficinaController(IOficinaService oficinaService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly IOficinaService _oficinaService = oficinaService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;

        [HttpGet]
        public async Task<IEnumerable<OficinaDTO>> GetAll()
        {
            return await _oficinaService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<OficinaDTO> GetById(int id)
        {
            return await _oficinaService.GetById(id);
        }

        [HttpPost]
        public async Task<Oficina> Create(OficinaDTO oficinaDTO)
        {
            var oficina = await _oficinaService.Create(oficinaDTO);
            return _mapper.Map<Oficina>(oficina);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, OficinaDTO oficinaDTO)
        {
            await _oficinaService.Update(id, oficinaDTO);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _oficinaService.Delete(id);
        }
    }
}