using GlobalAccount.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Infra.Interfaces
{
    public interface IClientRepository
    {
        Task InsertClientAsync(Client request);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string cpf);
        Task<Client?> GetByCpfAsync(string cpf);
        Task<List<Client>> ListAllAsync();
    }
}
