using GlobalAccount.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Tests.Validations
{
    public class CustomDateTest
    {
        private readonly CustomDate _validator = new();

        [Theory]
        [InlineData("01/01/2000")]
        [InlineData("29/02/2020")]
        [InlineData("31/12/1999")]
        public void Deve_Aceitar_Data_Valida(string data)
        {
            var resultado = _validator.IsValid(data);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("2020-01-01")]
        [InlineData("01-01-2020")]
        [InlineData("31/04/2020")] // abril só tem 30 dias
        [InlineData("abcd")]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Recusar_Data_Invalida(string data)
        {
            var resultado = _validator.IsValid(data);
            Assert.False(resultado);
        }
    }
}
