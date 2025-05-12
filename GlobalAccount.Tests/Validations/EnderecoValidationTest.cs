using GlobalAccount.Application.Validation;
using Xunit;

namespace GlobalAccount.Tests.Validations;

public class EnderecoValidationTest
{
    private readonly EnderecoValidation _validator = new();

    [Fact]
    public void Deve_Aceitar_Endereco_Com_UF_SP()
    {
        var resultado = _validator.IsValid("Rua das Flores, 123 - São Paulo SP");
        Assert.True(resultado);
    }

    [Fact]
    public void Deve_Aceitar_Endereco_Com_UF_RJ()
    {
        var resultado = _validator.IsValid("Av. Brasil, 1000 - Centro, Rio de Janeiro RJ");
        Assert.True(resultado);
    }

    [Fact]
    public void Deve_Aceitar_Endereco_Com_UF_PE()
    {
        var resultado = _validator.IsValid("Alameda dos Anjos, 45 - Recife PE");
        Assert.True(resultado);
    }

    [Fact]
    public void Deve_Aceitar_Endereco_Com_UF_BA()
    {
        var resultado = _validator.IsValid("Rua A, Bairro B, Salvador BA");
        Assert.True(resultado);
    }

    [Fact]
    public void Deve_Aceitar_Endereco_Com_UF_PB()
    {
        var resultado = _validator.IsValid("Rodovia BR-101, Km 210, João Pessoa PB");
        Assert.True(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Endereco_Sem_UF()
    {
        var resultado = _validator.IsValid("Rua Sem Estado");
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Endereco_Com_UF_Invalida()
    {
        var resultado = _validator.IsValid("Endereço com estado inexistente XX");
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Endereco_Vazio()
    {
        var resultado = _validator.IsValid("");
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Endereco_Nulo()
    {
        var resultado = _validator.IsValid(null);
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Endereco_Sem_Sigla()
    {
        var resultado = _validator.IsValid("Rua do Comércio, 200");
        Assert.False(resultado);
    }
}
