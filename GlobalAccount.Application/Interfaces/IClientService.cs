using GlobalAccount.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Application.Interfaces
{
    public interface IClientService
    {
        Task<ClientResponse> InsertClientAsync(ClientRequest request);
        Task UpdateClientAsync(ClientRequest request);
        Task DeleteClientAsync(string cpf);
        Task<ClientResponse?> GetClientByCpfAsync(string cpf);
        Task<List<ClientResponse>> ListAllClientsAsync();
    }
}
