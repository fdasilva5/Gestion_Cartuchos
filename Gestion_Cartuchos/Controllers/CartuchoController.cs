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

    public class CartuchoController(ICartuchoService cartuchoService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly ICartuchoService _cartuchoService = cartuchoService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;
    

    [HttpGet]
    public async Task<IEnumerable<CartuchoDTO>> GetAll()
    {
        return await _cartuchoService.GetAll();
    }

    [HttpGet("disponibles")]
    public async Task<IEnumerable<CartuchoDTO>> GetByEstadoDisponible()
    {
        return await _cartuchoService.GetByEstadoDisponible();
    }

    [HttpGet("compatibles/{impresoraId}")]
    public async Task<IEnumerable<ModeloDTO>> GetModelosCompatiblesConImpresora(int impresoraId)
    {
        return await _cartuchoService.GetModelosCompatiblesConImpresora(impresoraId);
    }


    
    [HttpGet("{id}")]
    public async Task<CartuchoDTO> GetById(int id)
    {
        return await _cartuchoService.GetById(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CartuchoDTO cartuchoDTO)
    {
        try
        {
            var cartucho = await _cartuchoService.Create(cartuchoDTO);
            return Ok(cartucho); 
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception )
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error inesperado." });
        }
    }


    [HttpPut("{id}")]
    public async Task Update(int id, CartuchoDTO cartuchoDTO)
    {
        await _cartuchoService.Update(id, cartuchoDTO);
    }

    [HttpPut("estadoEnRecarga/{id}")]
    public async Task<IActionResult> UpdateEstado(int id)
    {
        await _cartuchoService.ChangeEstadoToEnRecarga(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _cartuchoService.Delete(id);
    }
}
}
