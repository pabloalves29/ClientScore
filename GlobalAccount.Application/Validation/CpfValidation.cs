using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Application.Validation
{
    public class CpfValidation: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cpf = value as string;

            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11) return false;

            // Verifica se todos os caracteres são dígitos numéricos
            if (!cpf.All(char.IsDigit)) return false;

            // Verifica se todos os dígitos são iguais
            if (cpf.Distinct().Count() == 1) return false;

            var tempCpf = cpf.Substring(0, 9);
            var digitos = cpf.Substring(9, 2);

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (tempCpf[i] - '0') * (10 - i);

            int resto = soma % 11;
            int dig1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;
            tempCpf += dig1;
            for (int i = 0; i < 10; i++)
                soma += (tempCpf[i] - '0') * (11 - i);

            resto = soma % 11;
            int dig2 = resto < 2 ? 0 : 11 - resto;

            return digitos == $"{dig1}{dig2}";
        }
    }
}
