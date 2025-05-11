using GlobalAccount.Application.DTOs;
using GlobalAccount.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GlobalAccount.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientController> _logger;

    public ClientController(IClientService clientService, ILogger<ClientController> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Insert([FromBody] ClientRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Iniciando cadastro do cliente CPF: {Cpf}", request.Cpf);

            var resultado = await _clientService.InsertClientAsync(request);

            _logger.LogInformation("Cliente CPF: {Cpf} cadastrado com sucesso", request.Cpf);
            return Ok(resultado); // 200
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cadastrar cliente CPF: {Cpf}", request.Cpf);
            return StatusCode(500, new { erro = "Erro ao processar requisição.", detalhes = ex.Message });
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ClientRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Atualizando cliente CPF: {Cpf}", request.Cpf);
            await _clientService.UpdateClientAsync(request);
            _logger.LogInformation("Cliente CPF: {Cpf} atualizado com sucesso", request.Cpf);

            return Ok(new { mensagem = $"Cliente CPF {request.Cpf} atualizado com sucesso." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente CPF: {Cpf}", request.Cpf);
            return StatusCode(500, new { erro = "Erro ao atualizar cliente.", detalhes = ex.Message });
        }
    }


    [HttpGet("{cpf}")]
    [Authorize]
    public async Task<IActionResult> FindByCPF(string cpf)
    {
        try
        {
            _logger.LogInformation("Buscando cliente por CPF: {Cpf}", cpf);
            var resultado = await _clientService.GetClientByCpfAsync(cpf);

            if (resultado == null)
            {
                _logger.LogWarning("Cliente CPF: {Cpf} não encontrado", cpf);
                return NotFound(); // 404
            }

            _logger.LogInformation("Cliente CPF: {Cpf} encontrado com sucesso", cpf);
            return Ok(resultado); // 200
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar cliente CPF: {Cpf}", cpf);
            return StatusCode(500, new { erro = "Erro ao buscar cliente.", detalhes = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ListAll()
    {
        try
        {
            _logger.LogInformation("Listando todos os clientes");
            var lista = await _clientService.ListAllClientsAsync();

            _logger.LogInformation("Listagem de clientes concluída. Total: {Quantidade}", lista.Count);
            return Ok(lista); // 200
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar clientes");
            return StatusCode(500, new { erro = "Erro ao listar clientes.", detalhes = ex.Message });
        }
    }

    [HttpDelete("{cpf}")]
    [Authorize]
    public async Task<IActionResult> Delete(string cpf)
    {
        try
        {
            _logger.LogInformation("Excluindo cliente CPF: {Cpf}", cpf);
            await _clientService.DeleteClientAsync(cpf);
            _logger.LogInformation("Cliente CPF: {Cpf} excluído com sucesso", cpf);

            return Ok(new { mensagem = $"Cliente CPF {cpf} excluído com sucesso." });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "CPF não encontrado: {Cpf}", cpf);
            return NotFound(new { erro = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir cliente CPF: {Cpf}", cpf);
            return StatusCode(500, new { erro = "Erro ao excluir cliente.", detalhes = ex.Message });
        }
    }

}
