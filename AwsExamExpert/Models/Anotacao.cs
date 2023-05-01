namespace AwsExamExpert.Models
{
    public class Anotacao
    {
        public int CodigoAnotacao { get; set; }
        public string Texto { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoPergunta { get; set; }

        public Pergunta Pergunta { get; set; }
        public Usuario Usuario { get; set; }
    }
}
