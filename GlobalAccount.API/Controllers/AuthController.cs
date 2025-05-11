using GlobalAccount.Application.DTOs;
using GlobalAccount.Infra.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GlobalAccount.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(TokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Gera um token JWT com base em username e senha.
    /// </summary>
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] AuthRequest request)
    {
        _logger.LogInformation("Tentando autenticar usuário: {Username}", request.Username);

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            _logger.LogWarning("Tentativa de autenticação sem usuário informado");
            return BadRequest("Usuário é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Tentativa de autenticação sem senha informada");
            return BadRequest("Senha é obrigatória.");
        }

        try
        {
            var token = _tokenService.GenerateToken(request.Username, request.Password);

            if (token == null)
            {
                _logger.LogWarning("Falha de autenticação para usuário: {Username}", request.Username);
                return Unauthorized("Credenciais inválidas.");
            }

            _logger.LogInformation("Token gerado com sucesso para usuário: {Username}", request.Username);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar token para usuário: {Username}", request.Username);
            return StatusCode(500, new { erro = "Erro interno ao gerar token.", detalhes = ex.Message });
        }
    }
}
