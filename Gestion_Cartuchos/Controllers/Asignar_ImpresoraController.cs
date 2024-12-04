using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Asignar_ImpresoraController : ControllerBase
    {
        private readonly IAsignar_Impresora_Service _asignar_ImpresoraService;
        private readonly IMapper _mapper;
        private readonly Gestion_Cartuchos_Context _context;

        public Asignar_ImpresoraController(IAsignar_Impresora_Service asignar_ImpresoraService, IMapper mapper, Gestion_Cartuchos_Context context)
        {
            _asignar_ImpresoraService = asignar_ImpresoraService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Asignar_Impresora_DTO>> GetAll()
        {
            return await _asignar_ImpresoraService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Asignar_Impresora_DTO> GetById(int id)
        {
            return await _asignar_ImpresoraService.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Asignar_Impresora_DTO asignar_Impresora_DTO)
        {
            try
            {
                var result = await _asignar_ImpresoraService.Create(asignar_Impresora_DTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

      

        [HttpPost("desasignar")]
        public async Task DesasignarCartucho(int cartuchoId)
        {
            await _asignar_ImpresoraService.DesasignarCartucho(cartuchoId);
        }
    }
}