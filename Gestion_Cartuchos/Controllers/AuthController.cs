using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Models;
using Models.DTOs; 

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


         [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            // Crear el nuevo usuario
            var user = new ApplicationUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                 NombreCompleto = registerDTO.NombreCompleto  // Asegúrate de que esta propiedad se asigna

            };

            // Crear el usuario en la base de datos
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                // Si la creación es exitosa, puedes devolver un mensaje de éxito
                return Ok(new { Message = "User registered successfully" });
            }

            // Si la creación falla, devolver los errores
            return BadRequest(new { Message = "User registration failed", Errors = result.Errors });
        }

        // Endpoint para login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return Ok(new { Message = "Login successful" });
                }
                else
                {
                    return Unauthorized(new { Message = "Invalid credentials" });
                }
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }

        // Endpoint para logout (cerrar sesión)
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
