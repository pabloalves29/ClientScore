using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Application.Validation
{
    public class TelefoneValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var telefone = value as string;
            return telefone != null && System.Text.RegularExpressions.Regex.IsMatch(telefone, @"^\d{2}\d{8,9}$");
        }
    }
}
