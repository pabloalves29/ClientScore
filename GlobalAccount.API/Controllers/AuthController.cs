using GlobalAccount.Application.DTOs;
using GlobalAccount.Infra.Security;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAccount.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Gera um token JWT com base no e-mail.
        /// </summary>
        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] AuthRequest request)
        {
            // Aqui não há autenticação real, apenas geração do token.
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("E-mail é obrigatório.");

            var token = _tokenService.GenerateToken(request.Email);

            return Ok(new { token });
        }
    }
}

