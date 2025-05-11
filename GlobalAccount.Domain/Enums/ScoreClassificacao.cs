using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Domain.Enums
{
    public enum ScoreClassificacao
    {
        Mau = 1,
        Regular = 2,
        Bom = 3
    }

    public static class ScoreClassificacaoExtensions
    {
        public static string Descricao(this ScoreClassificacao classificacao)
        {
            return classificacao switch
            {
                ScoreClassificacao.Bom => "Bom Cliente",
                ScoreClassificacao.Regular => "Cliente Regular",
                ScoreClassificacao.Mau => "Mau Cliente",
                _ => "Desconhecido"
            };
        }
    }
}