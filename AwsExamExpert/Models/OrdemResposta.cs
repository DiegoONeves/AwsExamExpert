namespace AwsExamExpert.Models
{
    public class OrdemResposta
    {
        public int CodigoSimulado  { get; set; }

        public int CodigoPergunta { get; set; }
        public int CodigoQuestao { get; set; }
        public int CodigoResposta { get; set; }

        public string Ordem { get; set; }

        public override string ToString()
        {
            return $"CodigoSimulado: {CodigoSimulado} - CodigoPergunta: {CodigoPergunta} - CodigoQuestao: {CodigoQuestao} - CodigoResposta: {CodigoResposta} - Ordem: {Ordem}";
        }
    }
}
