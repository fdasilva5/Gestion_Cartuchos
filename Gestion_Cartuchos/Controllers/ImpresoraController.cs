using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using Models;
using Models.DTOs;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImpresoraController(IImpresoraService impresoraService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly IImpresoraService _impresoraService = impresoraService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _impresoraService.GetAll();
        return Ok(result);
    }

    [HttpGet("modelos")]
    public async Task<IEnumerable<ModeloDTO>> GetCompatibleModelos(int impresoraId)
    {
        return await _impresoraService.GetCompatibleModelos(impresoraId);
    }

    [HttpGet("{id}")]
    public async Task<ImpresoraDTO> GetById(int id)
    {
        return await _impresoraService.GetById(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ImpresoraDTO impresoraDTO)
    {
        try
        {
            var impresora = await _impresoraService.Create(impresoraDTO);
            return Ok(impresora);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
            {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error inesperado." });
        }
    }

    [HttpPut("{id}")]
    public async Task Update(int id, ImpresoraDTO impresoraDTO)
    {
        await _impresoraService.Update(id, impresoraDTO);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _impresoraService.Delete(id);
    }
}

}