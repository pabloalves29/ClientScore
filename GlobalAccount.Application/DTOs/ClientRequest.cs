using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GlobalAccount.Application.Validation;


namespace GlobalAccount.Application.DTOs
{
    public class ClientRequest
    {
        [Required]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [CustomDate(ErrorMessage = "A data deve estar no formato DD/MM/YYYY.")]
        public string DataNascimento { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter exatamente 11 dígitos numéricos.")]
        [CpfValidation(ErrorMessage = "CPF inválido.")]
        public string Cpf { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "O rendimento deve ser um valor positivo.")]
        public decimal RendimentoAnual { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "Estado deve conter exatamente 2 letras.")]
        public string Estado { get; set; }

        [Required]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone deve conter DDD e número (10 ou 11 dígitos).")]
        public string Telefone { get; set; }
    }
}
