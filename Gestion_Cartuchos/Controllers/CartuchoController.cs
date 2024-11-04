using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CartuchoContoller(CartuchoService cartuchoService, IMapper mapper, Gestion_Cartuchos_Context context) : ControllerBase
    {
        private readonly CartuchoService _cartuchoService = cartuchoService;
        private readonly IMapper _mapper = mapper;
        private readonly Gestion_Cartuchos_Context _context = context;
    

    [HttpGet]
    public async Task<IEnumerable<CartuchoDTO>> GetAll()
    {
        return await _cartuchoService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<CartuchoDTO> GetById(int id)
    {
        return await _cartuchoService.GetById(id);
    }

    [HttpPost]
    public async Task<Cartucho> Create(CartuchoDTO cartuchoDTO)
    {
        return await _cartuchoService.Create(cartuchoDTO);
    }

    [HttpPut("{id}")]
    public async Task Update(int id, CartuchoDTO cartuchoDTO)
    {
        await _cartuchoService.Update(id, cartuchoDTO);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _cartuchoService.Delete(id);
    }
}
}
