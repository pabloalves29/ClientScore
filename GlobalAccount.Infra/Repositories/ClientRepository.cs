using GlobalAccount.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using static System.Formats.Asn1.AsnWriter;
using GlobalAccount.Domain.Models;
using GlobalAccount.Domain.Enums;


namespace GlobalAccount.Infra.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task InsertClientAsync(Client request)
        {
            if ( (await GetByCpfAsync(request.Cpf)) != null)
                throw new InvalidOperationException("Já existe um cliente com este CPF: " + request.Cpf);

            const string query = @"
            INSERT INTO Clientes 
                (Nome, DataNascimento, Cpf, Email, RendimentoAnual, Estado, Telefone, Score, Classificacao)
            VALUES
                (@Nome, @DataNascimento, @Cpf, @Email, @RendimentoAnual, @Estado, @Telefone, @Score, @Classificacao)";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Nome", request.Nome);
            command.Parameters.AddWithValue("@DataNascimento", request.DataNascimento);
            command.Parameters.AddWithValue("@Cpf", request.Cpf);
            command.Parameters.AddWithValue("@Email", request.Email);
            command.Parameters.AddWithValue("@RendimentoAnual", request.RendimentoAnual);
            command.Parameters.AddWithValue("@Estado", request.Estado);
            command.Parameters.AddWithValue("@Telefone", request.Telefone);
            command.Parameters.AddWithValue("@Score", request.Score);
            command.Parameters.AddWithValue("@Classificacao", request.Classificacao.Descricao());

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(string cpf)
        {
            const string query = "DELETE FROM Clientes WHERE Cpf = @Cpf";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Cpf", cpf);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            const string query = @"
            UPDATE Clientes SET 
                Nome = @Nome,
                DataNascimento = @DataNascimento,
                Email = @Email,
                RendimentoAnual = @RendimentoAnual,
                Estado = @Estado,
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
            command.Parameters.AddWithValue("@Estado", client.Estado);
            command.Parameters.AddWithValue("@Telefone", client.Telefone);
            command.Parameters.AddWithValue("@Score", client.Score);
            command.Parameters.AddWithValue("@Classificacao", client.Classificacao.Descricao());

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Client>> ListAllAsync()
        {
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
                    estado: reader["Estado"].ToString(),
                    telefone: reader["Telefone"].ToString()
                ));
            }

            return lista;
        }

        public async Task<Client?> GetByCpfAsync(string cpf)
        {
            const string query = "SELECT * FROM Clientes WHERE Cpf = @Cpf";

            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Cpf", cpf);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Client(
                    nome: reader["Nome"].ToString(),
                    dataNascimento: Convert.ToDateTime(reader["DataNascimento"]),
                    cpf: reader["Cpf"].ToString(),
                    email: reader["Email"].ToString(),
                    rendimentoAnual: Convert.ToDecimal(reader["RendimentoAnual"]),
                    estado: reader["Estado"].ToString(),
                    telefone: reader["Telefone"].ToString()
                );
            }

            return null;
        }
    }
}
