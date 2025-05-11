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

namespace GlobalAccount.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientResponse> InsertClientAsync(ClientRequest request)
        {
            var client = MapRequestToEntity(request);

            await _clientRepository.InsertClientAsync(client);

            return MapEntityToResponse(client);
        }

        public async Task UpdateClientAsync(ClientRequest request)
        {
            var client = MapRequestToEntity(request);
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(string cpf)
        {
            await _clientRepository.DeleteAsync(cpf);
        }

        public async Task<ClientResponse?> GetClientByCpfAsync(string cpf)
        {
            var client = await _clientRepository.GetByCpfAsync(cpf);
            return client is null ? null : MapEntityToResponse(client);
        }

        public async Task<List<ClientResponse>> ListAllClientsAsync()
        {
            var clients = await _clientRepository.ListAllAsync();
            return clients.Select(MapEntityToResponse).ToList();
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
