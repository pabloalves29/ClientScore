using GlobalAccount.Domain.Enums;
using GlobalAccount.Domain.Models;
using GlobalAccount.Infra.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAccount.Infra.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(IConfiguration configuration, ILogger<ClientRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task InsertClientAsync(Client request)
        {
            try
            {
                _logger.LogInformation("Tentando inserir cliente CPF {Cpf}", request.Cpf);

                if ((await GetByCpfAsync(request.Cpf)) != null)
                    throw new InvalidOperationException($"Já existe um cliente com este CPF: {request.Cpf}");

                const string query = @"
                INSERT INTO Clientes 
                    (Nome, DataNascimento, Cpf, Email, RendimentoAnual, Endereco, Telefone, Score, Classificacao)
                VALUES
                    (@Nome, @DataNascimento, @Cpf, @Email, @RendimentoAnual, @Endereco, @Telefone, @Score, @Classificacao)";

                using var connection = new MySqlConnection(_connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Nome", request.Nome);
                command.Parameters.AddWithValue("@DataNascimento", request.DataNascimento);
                command.Parameters.AddWithValue("@Cpf", request.Cpf);
                command.Parameters.AddWithValue("@Email", request.Email);
                command.Parameters.AddWithValue("@RendimentoAnual", request.RendimentoAnual);
                command.Parameters.AddWithValue("@Endereco", request.Endereco);
                command.Parameters.AddWithValue("@Telefone", request.Telefone);
                command.Parameters.AddWithValue("@Score", request.Score);
                command.Parameters.AddWithValue("@Classificacao", request.Classificacao.Descricao());

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Cliente CPF {Cpf} inserido com sucesso", request.Cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir cliente CPF {Cpf}", request.Cpf);
                throw;
            }
        }

        public async Task DeleteAsync(string cpf)
        {
            try
            {
                if ((await GetByCpfAsync(cpf)) == null)
                    throw new InvalidOperationException($"Não foi localizado nenhum cliente com o CPF: {cpf}");

                const string query = "DELETE FROM Clientes WHERE Cpf = @Cpf";

                using var connection = new MySqlConnection(_connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Cpf", cpf);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Cliente CPF {Cpf} excluído com sucesso", cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir cliente CPF {Cpf}", cpf);
                throw;
            }
        }

        public async Task UpdateAsync(Client client)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar cliente CPF {Cpf}", client.Cpf);

                const string query = @"
                UPDATE Clientes SET 
                    Nome = @Nome,
                    DataNascimento = @DataNascimento,
                    Email = @Email,
                    RendimentoAnual = @RendimentoAnual,
                    Endereco = @Endereco,
                    Telefone = @Telefone,
                    Score = @Score,
                    Classificacao = @Classificacao
                WHERE Cpf = @Cpf";

                using var connection = new MySqlConnection(_connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Nome", client.Nome);
                command.Parameters.AddWithValue("@DataNascimento", client.DataNascimento);
                command.Parameters.AddWithValue("@Cpf", client.Cpf);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@RendimentoAnual", client.RendimentoAnual);
                command.Parameters.AddWithValue("@Endereco", client.Endereco);
                command.Parameters.AddWithValue("@Telefone", client.Telefone);
                command.Parameters.AddWithValue("@Score", client.Score);
                command.Parameters.AddWithValue("@Classificacao", client.Classificacao.Descricao());

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Cliente CPF {Cpf} atualizado com sucesso", client.Cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente CPF {Cpf}", client.Cpf);
                throw;
            }
        }

        public async Task<List<Client>> ListAllAsync()
        {
            try
            {
                _logger.LogInformation("Listando todos os clientes");

                const string query = "SELECT * FROM Clientes";
                using var connection = new MySqlConnection(_connectionString);
                using var command = new MySqlCommand(query, connection);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                var lista = new List<Client>();

                while (await reader.ReadAsync())
                {
                    lista.Add(new Client(
                        nome: reader["Nome"].ToString(),
                        dataNascimento: Convert.ToDateTime(reader["DataNascimento"]),
                        cpf: reader["Cpf"].ToString(),
                        email: reader["Email"].ToString(),
                        rendimentoAnual: Convert.ToDecimal(reader["RendimentoAnual"]),
                        endereco: reader["Endereco"].ToString(),
                        telefone: reader["Telefone"].ToString()
                    ));
                }

                _logger.LogInformation("Listagem de clientes concluída com sucesso");
                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar clientes");
                throw;
            }
        }

        public async Task<Client?> GetByCpfAsync(string cpf)
        {
            try
            {
                _logger.LogInformation("Buscando cliente pelo CPF {Cpf}", cpf);

                const string query = "SELECT * FROM Clientes WHERE Cpf = @Cpf";
                using var connection = new MySqlConnection(_connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Cpf", cpf);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    _logger.LogInformation("Cliente CPF {Cpf} encontrado", cpf);

                    return new Client(
                        nome: reader["Nome"].ToString(),
                        dataNascimento: Convert.ToDateTime(reader["DataNascimento"]),
                        cpf: reader["Cpf"].ToString(),
                        email: reader["Email"].ToString(),
                        rendimentoAnual: Convert.ToDecimal(reader["RendimentoAnual"]),
                        endereco: reader["Endereco"].ToString(),
                        telefone: reader["Telefone"].ToString()
                    );
                }

                _logger.LogWarning("Cliente CPF {Cpf} não encontrado", cpf);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cliente CPF {Cpf}", cpf);
                throw;
            }
        }
    }
}
