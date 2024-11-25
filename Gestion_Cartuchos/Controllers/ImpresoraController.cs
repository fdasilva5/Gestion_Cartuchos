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

    public class ImpresoraController(ImpresoraService impresoraService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly ImpresoraService _impresoraService = impresoraService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;

    [HttpGet]
    public async Task<IEnumerable<ImpresoraDTO>> GetAll()
    {
        return await _impresoraService.GetAll();
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
    public async Task<Impresora> Create(ImpresoraDTO impresoraDTO)
    {
        var impresora = await _impresoraService.Create(impresoraDTO);
        return _mapper.Map<Impresora>(impresora);
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