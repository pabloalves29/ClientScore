using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace GlobalAccount.Application.Validation
{
    public class EnderecoValidation : ValidationAttribute
    {
        private static readonly string[] uf = {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
            "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
            "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        public EnderecoValidation()
        {
            ErrorMessage = "Endereço deve conter uma UF válida (ex: 'Av. Brasil, 123 - Curitiba PR').";
        }

        public override bool IsValid(object value)
        {
            var endereco = value as string;
            if (string.IsNullOrWhiteSpace(endereco)) return false;

            foreach (var estado in uf)
            {
                if (Regex.IsMatch(endereco.Trim(), @$"\b{estado}$", RegexOptions.IgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
