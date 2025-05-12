using GlobalAccount.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GlobalAccount.Tests.DTOs;

public class ClientRequestValidationTest
{
    [Fact]
    public void Deve_Retornar_Erro_Para_Email_Invalido()
    {
        var dto = new ClientRequest
        {
            Nome = "João",
            DataNascimento = "10/10/1990",
            Cpf = "42811229144",
            Email = "email-invalido",
            RendimentoAnual = 50000,
            Endereco = "Rua das Palmeiras, 100 - São Paulo SP",
            Telefone = "11999999999"
        };

        var results = Validar(dto);

        Assert.False(results.isValid);
        Assert.Contains(results.errors, r => r.ErrorMessage.Contains("Email"));
    }

    [Fact]
    public void Deve_Retornar_Erro_Para_Endereco_Sem_UF()
    {
        var dto = new ClientRequest
        {
            Nome = "João",
            DataNascimento = "10/10/1990",
            Cpf = "42811229144",
            Email = "joao@email.com",
            RendimentoAnual = 50000,
            Endereco = "Rua das Palmeiras, 100 - São Paulo",
            Telefone = "11999999999"
        };

        var results = Validar(dto);

        Assert.False(results.isValid);
        Assert.Contains(results.errors, r => r.ErrorMessage.Contains("Endereço"));
    }

    [Fact]
    public void Deve_Retornar_Erro_Para_Telefone_Com_Formato_Invalido()
    {
        var dto = new ClientRequest
        {
            Nome = "Ana",
            DataNascimento = "10/10/1990",
            Cpf = "42811229144",
            Email = "ana@email.com",
            RendimentoAnual = 60000,
            Endereco = "Rua XPTO, 200 - Belo Horizonte MG",
            Telefone = "(11) 99999-9999"
        };

        var results = Validar(dto);

        Assert.False(results.isValid);
        Assert.Contains(results.errors, r => r.ErrorMessage.Contains("Telefone"));
    }

    [Fact]
    public void Deve_Validar_Corretamente_Dto_Valido()
    {
        var dto = new ClientRequest
        {
            Nome = "Maria",
            DataNascimento = "01/01/1990",
            Cpf = "61411340094",
            Email = "maria@email.com",
            RendimentoAnual = 70000,
            Endereco = "Av. Brasil, 500 - Rio de Janeiro RJ",
            Telefone = "21912345678"
        };

        var results = Validar(dto);

        Assert.True(results.isValid);
        Assert.Empty(results.errors);
    }

    [Fact]
    public void Deve_Retornar_Erro_Para_CPF_Invalido()
    {
        var dto = new ClientRequest
        {
            Nome = "Lucas",
            DataNascimento = "01/01/1990",
            Cpf = "12345678900", // inválido
            Email = "lucas@email.com",
            RendimentoAnual = 70000,
            Endereco = "Rua Teste, 1 - Porto Alegre RS",
            Telefone = "51987654321"
        };

        var results = Validar(dto);

        Assert.False(results.isValid);
        Assert.Contains(results.errors, r => r.ErrorMessage.Contains("CPF"));
    }

    private (bool isValid, List<ValidationResult> errors) Validar(ClientRequest dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, context, results, true);
        return (isValid, results);
    }
}
