using GlobalAccount.Domain.Enums;

namespace GlobalAccount.Domain.Models;

public class Client
{
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public decimal RendimentoAnual { get; private set; }
    public string Endereco { get; private set; } 
    public string Telefone { get; private set; }  

    public int Score { get; private set; }
    public ScoreClassificacao Classificacao { get; private set; }

    public Client(string nome, DateTime dataNascimento, string cpf, string email, decimal rendimentoAnual, 
        string endereco, string telefone)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Email = email;
        RendimentoAnual = rendimentoAnual;
        Endereco = endereco;
        Telefone = telefone;

        CalcularScoreEClassificacao();
    }

    private int CalcularIdade()
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - DataNascimento.Year;
        if (DataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }

    private void CalcularScoreEClassificacao()
    {
        Score = ObterPontuacaoIdade(CalcularIdade()) + ObterPontuacaoRendimento(RendimentoAnual);
        Classificacao = ClassificarScore(Score);
    }

    private static int ObterPontuacaoIdade(int idade)
    {
        return idade switch
        {
            > 40 => 200,
            >= 25 => 150,
            _ => 50
        };
    }

    private static int ObterPontuacaoRendimento(decimal rendimento)
    {
        return rendimento switch
        {
            > 120000 => 300,
            >= 60000 => 200,
            _ => 100
        };
    }

    private static ScoreClassificacao ClassificarScore(int score)
    {
        return score switch
        {
            >= 450 => ScoreClassificacao.Bom,
            >= 300 => ScoreClassificacao.Regular,
            _ => ScoreClassificacao.Mau
        };
    }
}
