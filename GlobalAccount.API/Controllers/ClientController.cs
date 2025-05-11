using GlobalAccount.Application.DTOs;
using GlobalAccount.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAccount.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Cadastra um novo cliente e retorna o score e a classificação.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Insert([FromBody] ClientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _clientService.InsertClientAsync(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao processar requisição.", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] ClientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _clientService.UpdateClientAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao atualizar cliente.", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Retorna os dados de um cliente com base no CPF.
        /// </summary>
        [HttpGet("{cpf}")]
        [Authorize]
        public async Task<IActionResult> FindByCPF(string cpf)
        {
            try
            {
                var resultado = await _clientService.GetClientByCpfAsync(cpf);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao buscar cliente.", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os clientes cadastrados.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                var lista = await _clientService.ListAllClientsAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao listar clientes.", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Exclui um cliente com base no CPF.
        /// </summary>
        [HttpDelete("{cpf}")]
        [Authorize]
        public async Task<IActionResult> Delete(string cpf)
        {
            try
            {
                await _clientService.DeleteClientAsync(cpf);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao excluir cliente.", detalhes = ex.Message });
            }
        }
    }
}
