using System.ComponentModel.DataAnnotations;
using GlobalAccount.Application.Validation;
using Xunit;


namespace GlobalAccount.Tests.Validations
{
    public class CpfValidationTest
    {
        private readonly CpfValidation _validator = new();

        [Theory]
        [InlineData("12345678909")] // válido
        [InlineData("11144477735")] // válido
        public void Deve_Aceitar_Cpf_Valido(string cpf)
        {
            var resultado = _validator.IsValid(cpf);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("12345678900")] // dígito inválido
        [InlineData("00000000000")] // todos iguais
        [InlineData("abcdefghijk")] // letras
        [InlineData("123")]         // muito curto
        [InlineData("")]            // vazio
        [InlineData(null)]
        public void Deve_Recusar_Cpf_Invalido(string cpf)
        {
            var resultado = _validator.IsValid(cpf);
            Assert.False(resultado);
        }
    }
}
