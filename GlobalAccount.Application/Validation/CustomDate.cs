using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Application.Validation
{
    public class CustomDate: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not string input) return false;

            return DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
