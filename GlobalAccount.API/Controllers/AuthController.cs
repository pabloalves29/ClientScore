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
            if (string.IsNullOrWhiteSpace(request.Username))
                return BadRequest("Usuário é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Senha é obrigatória.");

            var token = _tokenService.GenerateToken(request.Username, request.Password);

            if (token == null)
                return Unauthorized("Credenciais inválidas.");

            return Ok(new { token });
        }
    }
}

