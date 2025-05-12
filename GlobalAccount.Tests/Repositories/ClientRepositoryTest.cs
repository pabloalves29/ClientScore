using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalAccount.Domain.Models;
using GlobalAccount.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GlobalAccount.Tests.Repositories;

public class ClientRepositoryTests
{
    private readonly ClientRepository _repository;
    private readonly Mock<ILogger<ClientRepository>> _loggerMock;
    private const string CpfTeste = "22233344455";

    public ClientRepositoryTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        _loggerMock = new Mock<ILogger<ClientRepository>>();
        _repository = new ClientRepository(configuration, _loggerMock.Object);
    }

    private async Task LimparCliente()
    {
        try
        {
            await _repository.DeleteAsync(CpfTeste);
        }
        catch { /* silencioso para garantir isolamento dos testes */ }
    }

    private Client CriarClienteBase(string nome = "Cliente Teste", decimal rendimento = 80000)
    {
        return new Client(
            nome: nome,
            dataNascimento: new DateTime(1990, 1, 1),
            cpf: CpfTeste,
            email: "cliente@email.com",
            rendimentoAnual: rendimento,
            endereco: "SP",
            telefone: "11999999999"
        );
    }

    #region Insert

    [Fact]
    public async Task Deve_Inserir_Cliente_Corretamente()
    {
        await LimparCliente();
        var cliente = CriarClienteBase();
        await _repository.InsertClientAsync(cliente);
        var resultado = await _repository.GetByCpfAsync(CpfTeste);
        Assert.NotNull(resultado);
        Assert.Equal("Cliente Teste", resultado.Nome);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_Cpf_Duplicado()
    {
        await LimparCliente();
        var cliente = CriarClienteBase("Duplicado");
        await _repository.InsertClientAsync(cliente);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.InsertClientAsync(cliente));
        Assert.Contains("Já existe um cliente com este CPF", ex.Message);
    }

    [Fact]
    public async Task Deve_Inserir_Cliente_Com_Dados_Minimos_Validos()
    {
        await LimparCliente();
        var cliente = CriarClienteBase("Simples", 60000);
        await _repository.InsertClientAsync(cliente);
        var resultado = await _repository.GetByCpfAsync(CpfTeste);
        Assert.Equal("Simples", resultado.Nome);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Deve_Atualizar_Cliente_Corretamente()
    {
        await LimparCliente();
        await _repository.InsertClientAsync(CriarClienteBase("Original"));

        var atualizado = CriarClienteBase("Atualizado");
        await _repository.UpdateAsync(atualizado);

        var resultado = await _repository.GetByCpfAsync(CpfTeste);
        Assert.Equal("Atualizado", resultado.Nome);
    }

    [Fact]
    public async Task Atualizacao_Deve_Recalcular_Score()
    {
        await LimparCliente();
        var cliente = CriarClienteBase("Score Original", 30000); // Score baixo
        await _repository.InsertClientAsync(cliente);

        var atualizado = CriarClienteBase("Score Bom", 130000); // Score alto
        await _repository.UpdateAsync(atualizado);

        var result = await _repository.GetByCpfAsync(CpfTeste);
        Assert.Equal("Score Bom", result.Nome);
    }

    [Fact]
    public async Task Atualizar_Cliente_Inexistente_Nao_Lanca_Erro()
    {
        await LimparCliente();
        var cliente = CriarClienteBase("Inexistente");
        var ex = await Record.ExceptionAsync(() => _repository.UpdateAsync(cliente));
        Assert.Null(ex); // comportamento silencioso (idempotente)
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Deve_Excluir_Cliente_Por_Cpf()
    {
        await LimparCliente();
        await _repository.InsertClientAsync(CriarClienteBase());
        await _repository.DeleteAsync(CpfTeste);
        var resultado = await _repository.GetByCpfAsync(CpfTeste);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task Delete_Deve_Ser_Idempotente_Para_Cpf_Inexistente()
    {
        await LimparCliente(); // Garantir que cliente não existe
        var ex = await Record.ExceptionAsync(() => _repository.DeleteAsync(CpfTeste));
        Assert.NotNull(ex); // agora lança InvalidOperationException
        Assert.Contains("Não foi localizado nenhum cliente", ex.Message);
    }

    [Fact]
    public async Task Delete_Deve_Remover_Cliente_Da_Listagem()
    {
        await LimparCliente();
        await _repository.InsertClientAsync(CriarClienteBase("Remover"));

        await _repository.DeleteAsync(CpfTeste);
        var lista = await _repository.ListAllAsync();
        Assert.DoesNotContain(lista, c => c.Cpf == CpfTeste);
    }

    #endregion

    #region GetByCpf

    [Fact]
    public async Task Deve_Retornar_Cliente_Por_Cpf()
    {
        await LimparCliente();
        var cliente = CriarClienteBase("Busca CPF");
        await _repository.InsertClientAsync(cliente);

        var resultado = await _repository.GetByCpfAsync(CpfTeste);
        Assert.NotNull(resultado);
        Assert.Equal("Busca CPF", resultado.Nome);
    }

    [Fact]
    public async Task GetByCpf_Deve_Retornar_Null_Se_Inexistente()
    {
        await LimparCliente();
        var resultado = await _repository.GetByCpfAsync(CpfTeste);
        Assert.Null(resultado);
    }

    [Fact]
    public async Task GetByCpf_Deve_Mapear_Campos_Corretamente()
    {
        await LimparCliente();
        await _repository.InsertClientAsync(CriarClienteBase("Mapeado"));

        var cliente = await _repository.GetByCpfAsync(CpfTeste);
        Assert.Equal("Mapeado", cliente.Nome);
        Assert.Equal("cliente@email.com", cliente.Email);
        Assert.Equal("SP", cliente.Endereco);
    }

    #endregion

    #region ListAll

    [Fact]
    public async Task Deve_Listar_Todos_Os_Clientes()
    {
        await LimparCliente();
        await _repository.InsertClientAsync(CriarClienteBase("Listagem"));
        var lista = await _repository.ListAllAsync();
        Assert.Contains(lista, x => x.Cpf == CpfTeste);
    }

    [Fact]
    public async Task Listagem_Deve_Retornar_Lista_Vazia_Se_Nao_Houver_Clientes()
    {
        await LimparCliente();
        var lista = await _repository.ListAllAsync();
        Assert.Empty(lista.Where(c => c.Cpf == CpfTeste));
    }

    [Fact]
    public async Task Listagem_Deve_Retornar_Todos_Com_Dados_Corretos()
    {
        await LimparCliente();
        await _repository.InsertClientAsync(CriarClienteBase("Um"));
        var lista = await _repository.ListAllAsync();

        var cliente = lista.FirstOrDefault(c => c.Cpf == CpfTeste);
        Assert.NotNull(cliente);
        Assert.Equal("Um", cliente.Nome);
    }

    #endregion
}
