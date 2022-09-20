namespace AwsExamExpert.Models
{
    public class Resposta
    {
        public int CodigoResposta { get; set; }
        public int CodigoPergunta { get; set; }

        public string Ordem { get; set; }
        public string Texto { get; set; }

        public bool Correta { get; set; }

        public bool Ativo { get; set; }
        public decimal Pontuacao { get; set; }
        public bool Respondida { get; set; }

        public Pergunta Pergunta { get; set; }
    }
}
