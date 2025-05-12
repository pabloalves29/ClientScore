using GlobalAccount.Application.Validation;
using Xunit;

namespace GlobalAccount.Tests.Validations;

public class TelefoneValidationTest
{
    private readonly TelefoneValidation _validator = new();

    [Fact]
    public void Deve_Aceitar_Telefone_Com_DDD_E_Nove_Digitos()
    {
        var resultado = _validator.IsValid("11999999999"); // DDD 11 + 9 dígitos
        Assert.True(resultado);
    }

    [Fact]
    public void Deve_Aceitar_Telefone_Com_DDD_E_Oito_Digitos()
    {
        var resultado = _validator.IsValid("1134567890"); // DDD 11 + 8 dígitos
        Assert.True(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Sem_DDD()
    {
        var resultado = _validator.IsValid("999999999"); // sem DDD
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Com_Menos_Digitos()
    {
        var resultado = _validator.IsValid("11345678"); // DDD + 6 dígitos
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Com_Mais_De_11_Digitos()
    {
        var resultado = _validator.IsValid("119999999999"); // 12 dígitos
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Com_Letras()
    {
        var resultado = _validator.IsValid("11abcdefghi");
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Com_Espacos()
    {
        var resultado = _validator.IsValid("11 99999 9999");
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Vazio()
    {
        var resultado = _validator.IsValid("");
        Assert.False(resultado);
    }

    [Fact]
    public void Nao_Deve_Aceitar_Telefone_Nulo()
    {
        var resultado = _validator.IsValid(null);
        Assert.False(resultado);
    }

    [Fact]
    public void Deve_Aceitar_Telefone_De_Outro_DDD()
    {
        var resultado = _validator.IsValid("85987654321"); // DDD 85
        Assert.True(resultado);
    }
}
