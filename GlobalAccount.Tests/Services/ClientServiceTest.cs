using GlobalAccount.Application.DTOs;
using GlobalAccount.Application.Services;
using GlobalAccount.Domain.Enums;
using GlobalAccount.Domain.Models;
using GlobalAccount.Infra.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GlobalAccount.Tests.Services;

public class ClientServiceTest
{
    private readonly Mock<IClientRepository> _mockRepo;
    private readonly Mock<ILogger<ClientService>> _mockLogger;
    private readonly ClientService _service;

    public ClientServiceTest()
    {
        _mockRepo = new Mock<IClientRepository>();
        _mockLogger = new Mock<ILogger<ClientService>>();
        _service = new ClientService(_mockRepo.Object, _mockLogger.Object);
    }

    #region Insert

    [Fact]
    public async Task Insert_Deve_Criar_Cliente_Bom()
    {
        var request = CriarRequest("Joana", "01/01/1980", "11111111111", 150000);
        var result = await _service.InsertClientAsync(request);

        Assert.Equal("Joana", result.Nome);
        Assert.Equal(ScoreClassificacao.Bom.Descricao(), result.Classificacao);
        _mockRepo.Verify(x => x.InsertClientAsync(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public async Task Insert_Deve_Criar_Cliente_Regular()
    {
        var request = CriarRequest("Ana", "01/01/1995", "22222222222", 80000);
        var result = await _service.InsertClientAsync(request);

        Assert.Equal("Cliente Regular", result.Classificacao);
        Assert.Equal(350, result.Score);
    }

    [Fact]
    public async Task Insert_Deve_Criar_Cliente_Mau()
    {
        var request = CriarRequest("Paulo", "01/01/2006", "33333333333", 30000);
        var result = await _service.InsertClientAsync(request);

        Assert.Equal("Mau Cliente", result.Classificacao);
        Assert.Equal(150, result.Score);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_Deve_Executar_Sem_Exception()
    {
        var request = CriarRequest("Atualizado", "01/01/1990", "44444444444", 90000);
        await _service.UpdateClientAsync(request);

        _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public async Task Update_Deve_Repassar_Dados_Corretos()
    {
        Client recebido = null;
        _mockRepo.Setup(x => x.UpdateAsync(It.IsAny<Client>()))
                 .Callback<Client>(c => recebido = c);

        var request = CriarRequest("Bruno", "01/01/1993", "55555555555", 80000);
        await _service.UpdateClientAsync(request);

        Assert.Equal("Bruno", recebido.Nome);
        Assert.Equal("55555555555", recebido.Cpf);
    }

    [Fact]
    public async Task Update_Deve_Recalcular_Score()
    {
        var request = CriarRequest("Fernanda", "01/01/2002", "66666666666", 30000);
        await _service.UpdateClientAsync(request);

        _mockRepo.Verify(x => x.UpdateAsync(It.Is<Client>(c => c.Score == 150)), Times.Once);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_Deve_Chamar_Repositorio()
    {
        await _service.DeleteClientAsync("99999999999");
        _mockRepo.Verify(x => x.DeleteAsync("99999999999"), Times.Once);
    }

    [Fact]
    public async Task Delete_Deve_Aceitar_Cpf_Valido()
    {
        var cpf = "12345678901";
        await _service.DeleteClientAsync(cpf);
        _mockRepo.Verify(x => x.DeleteAsync(cpf), Times.Once);
    }

    [Fact]
    public async Task Delete_Nao_Deve_Lancar_Exception()
    {
        var ex = await Record.ExceptionAsync(() => _service.DeleteClientAsync("00000000000"));
        Assert.Null(ex);
    }

    #endregion

    #region GetByCpf

    [Fact]
    public async Task GetByCpf_Deve_Retornar_Null_Se_Nao_Encontrado()
    {
        _mockRepo.Setup(x => x.GetByCpfAsync("000")).ReturnsAsync((Client)null);
        var result = await _service.GetClientByCpfAsync("000");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCpf_Deve_Retornar_Cliente_Correto()
    {
        var entity = new Client("Lucas", DateTime.Today.AddYears(-30), "999", "lucas@email.com", 60000, "SP", "111111111");
        _mockRepo.Setup(x => x.GetByCpfAsync("999")).ReturnsAsync(entity);

        var result = await _service.GetClientByCpfAsync("999");

        Assert.NotNull(result);
        Assert.Equal("Lucas", result.Nome);
    }

    [Fact]
    public async Task GetByCpf_Deve_Mapear_Score_E_Descricao()
    {
        var entity = new Client("Júlia", DateTime.Today.AddYears(-28), "777", "julia@email.com", 75000, "MG", "88888");
        _mockRepo.Setup(x => x.GetByCpfAsync("777")).ReturnsAsync(entity);

        var result = await _service.GetClientByCpfAsync("777");

        Assert.Equal(350, result.Score);
        Assert.Equal("Cliente Regular", result.Classificacao);
    }

    #endregion

    #region ListAll

    [Fact]
    public async Task ListAll_Deve_Retornar_Lista_Vazia_Se_Nenhum_Cliente()
    {
        _mockRepo.Setup(x => x.ListAllAsync()).ReturnsAsync(new List<Client>());
        var result = await _service.ListAllClientsAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ListAll_Deve_Retornar_Clientes_Existentes()
    {
        var lista = new List<Client>
        {
            new("Fulano", DateTime.Today.AddYears(-50), "111", "a@a.com", 120000, "SP", "111"),
            new("Beltrano", DateTime.Today.AddYears(-20), "222", "b@b.com", 40000, "MG", "222")
        };

        _mockRepo.Setup(x => x.ListAllAsync()).ReturnsAsync(lista);

        var result = await _service.ListAllClientsAsync();
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Nome == "Fulano");
    }

    [Fact]
    public async Task ListAll_Deve_Mapear_Score_E_Descricao()
    {
        var lista = new List<Client>
        {
            new("Fernanda", DateTime.Today.AddYears(-45), "333", "f@x.com", 100000, "RJ", "777")
        };

        _mockRepo.Setup(x => x.ListAllAsync()).ReturnsAsync(lista);

        var result = await _service.ListAllClientsAsync();

        Assert.Single(result);
        Assert.Equal(400, result.First().Score);
        Assert.Equal("Cliente Regular", result.First().Classificacao);
    }

    #endregion

    #region Helpers

    private static ClientRequest CriarRequest(string nome, string dataNascimento, string cpf, decimal rendimento)
    {
        return new ClientRequest
        {
            Nome = nome,
            DataNascimento = dataNascimento,
            Cpf = cpf,
            Email = $"{nome.ToLower()}@email.com",
            Estado = "SP",
            Telefone = "11999999999",
            RendimentoAnual = rendimento
        };
    }

    #endregion
}
