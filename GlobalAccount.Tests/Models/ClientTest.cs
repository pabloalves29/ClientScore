using GlobalAccount.Domain.Enums;
using GlobalAccount.Domain.Models;
using Xunit;

namespace GlobalAccount.Tests.Domain;

public class ClientTest
{
    [Fact]
    public void Deve_Calcular_Bom_Cliente_Para_Idade_Acima_40_E_Renda_Alta()
    {
        var client = new Client(
            nome: "Joana",
            dataNascimento: DateTime.Today.AddYears(-45),
            cpf: "12345678901",
            email: "joana@email.com",
            rendimentoAnual: 150000,
            estado: "SP",
            telefone: "11999999999"
        );

        Assert.Equal(500, client.Score);
        Assert.Equal(ScoreClassificacao.Bom, client.Classificacao);
    }

    [Fact]
    public void Deve_Calcular_Regular_Cliente_Para_Idade_Entre_25_40_E_Renda_Media()
    {
        var client = new Client(
            nome: "Lucas",
            dataNascimento: DateTime.Today.AddYears(-30),
            cpf: "98765432100",
            email: "lucas@email.com",
            rendimentoAnual: 90000,
            estado: "RJ",
            telefone: "21999999999"
        );

        Assert.Equal(350, client.Score);
        Assert.Equal(ScoreClassificacao.Regular, client.Classificacao);
    }

    [Fact]
    public void Deve_Calcular_Mau_Cliente_Para_Idade_Baixa_E_Renda_Baixa()
    {
        var client = new Client(
            nome: "Carla",
            dataNascimento: DateTime.Today.AddYears(-20),
            cpf: "11122233344",
            email: "carla@email.com",
            rendimentoAnual: 40000,
            estado: "MG",
            telefone: "31999999999"
        );

        Assert.Equal(150, client.Score);
        Assert.Equal(ScoreClassificacao.Mau, client.Classificacao);
    }

    [Fact]
    public void Deve_Atribuir_Campos_Corretamente()
    {
        var nascimento = new DateTime(1995, 10, 10);
        var client = new Client(
            nome: "Pedro",
            dataNascimento: nascimento,
            cpf: "44455566677",
            email: "pedro@email.com",
            rendimentoAnual: 80000,
            estado: "PR",
            telefone: "41999999999"
        );

        Assert.Equal("Pedro", client.Nome);
        Assert.Equal(nascimento, client.DataNascimento);
        Assert.Equal("44455566677", client.Cpf);
        Assert.Equal("pedro@email.com", client.Email);
        Assert.Equal("PR", client.Estado);
        Assert.Equal("41999999999", client.Telefone);
    }

    [Fact]
    public void Deve_Calcular_Score_Borda_Exatamente_450()
    {
        var client = new Client(
            nome: "Ana",
            dataNascimento: DateTime.Today.AddYears(-30), // 150 pontos
            cpf: "88899977766",
            email: "ana@email.com",
            rendimentoAnual: 120001, // 300 pontos
            estado: "RS",
            telefone: "51999999999"
        );

        Assert.Equal(450, client.Score);
        Assert.Equal(ScoreClassificacao.Bom, client.Classificacao);
    }
}
