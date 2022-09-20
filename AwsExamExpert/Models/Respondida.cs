namespace AwsExamExpert.Models
{
    public class Respondida
    {
        public int CodigoResposta { get; set; }
        public int CodigoQuestao { get; set; }

        public Questao Questao { get; set; }
        public Resposta Resposta { get; set; }

        public override string ToString()
        {
            return $"CodigoResposta: {CodigoResposta} - CodigoQuestao: {CodigoQuestao}";
        }
    }
}
