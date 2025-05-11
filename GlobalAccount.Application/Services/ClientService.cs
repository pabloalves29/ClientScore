using GlobalAccount.Application.DTOs;
using GlobalAccount.Application.Interfaces;
using GlobalAccount.Domain.Models;
using GlobalAccount.Infra.Interfaces;
using GlobalAccount.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GlobalAccount.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        private readonly ILogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<ClientResponse> InsertClientAsync(ClientRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando inserção de cliente: {Cpf}", request.Cpf);
                var client = MapRequestToEntity(request);

                await _clientRepository.InsertClientAsync(client);

                _logger.LogInformation("Cliente inserido com sucesso: {Cpf}", client.Cpf);
                return MapEntityToResponse(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir cliente: {Cpf}", request.Cpf);
                throw;
            }
        }


        public async Task UpdateClientAsync(ClientRequest request)
        {
            try
            {
                _logger.LogInformation("Atualizando cliente: {Cpf}", request.Cpf);
                var client = MapRequestToEntity(request);
                await _clientRepository.UpdateAsync(client);
                _logger.LogInformation("Cliente atualizado: {Cpf}", request.Cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente: {Cpf}", request.Cpf);
                throw;
            }
        }

        public async Task DeleteClientAsync(string cpf)
        {
            try
            {
                _logger.LogInformation("Deletando cliente: {Cpf}", cpf);
                await _clientRepository.DeleteAsync(cpf);
                _logger.LogInformation("Cliente deletado: {Cpf}", cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar cliente: {Cpf}", cpf);
                throw;
            }
        }

        public async Task<ClientResponse?> GetClientByCpfAsync(string cpf)
        {
            try
            {
                _logger.LogInformation("Buscando cliente por CPF: {Cpf}", cpf);

                var client = await _clientRepository.GetByCpfAsync(cpf);

                if (client == null)
                {
                    _logger.LogWarning("Cliente não encontrado para o CPF: {Cpf}", cpf);
                    return null;
                }

                _logger.LogInformation("Cliente encontrado: {Cpf}", cpf);
                return MapEntityToResponse(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cliente por CPF: {Cpf}", cpf);
                throw;
            }
        }
        public async Task<List<ClientResponse>> ListAllClientsAsync()
        {
            try
            {
                _logger.LogInformation("Listando todos os clientes");
                var clients = await _clientRepository.ListAllAsync();

                var result = clients.Select(MapEntityToResponse).ToList();
                _logger.LogInformation("Total de clientes retornados: {Quantidade}", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar todos os clientes");
                throw;
            }
        }

        private static Client MapRequestToEntity(ClientRequest request)
        {
            return new Client(
                nome: request.Nome,
                dataNascimento: DateTime.ParseExact(request.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                cpf: request.Cpf,
                email: request.Email,
                rendimentoAnual: request.RendimentoAnual,
                estado: request.Estado,
                telefone: request.Telefone
            );
        }

        private static ClientResponse MapEntityToResponse(Client client)
        {
            return new ClientResponse
            {
                Nome = client.Nome,
                Score = client.Score,
                Classificacao = client.Classificacao.Descricao()
            };
        }
    }
}
