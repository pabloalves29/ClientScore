using GlobalAccount.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Tests.DTOs
{
    public class ClientRequestValidationTest
    {
        [Fact]
        public void Deve_Retornar_Erro_Para_Email_Invalido()
        {
            var dto = new ClientRequest
            {
                Nome = "João",
                DataNascimento = "10/10/1990",
                Cpf = "12345678909",
                Email = "email-invalido",
                RendimentoAnual = 50000,
                Estado = "SP",
                Telefone = "11999999999"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Email inválido"));
        }

        [Fact]
        public void Deve_Validar_Corretamente_Dto_Valido()
        {
            var dto = new ClientRequest
            {
                Nome = "Maria",
                DataNascimento = "01/01/1990",
                Cpf = "12345678909",
                Email = "maria@email.com",
                RendimentoAnual = 70000,
                Estado = "RJ",
                Telefone = "21912345678"
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.True(isValid);
        }
    }
}
