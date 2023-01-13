using System.Collections.Generic;

namespace AwsExamExpert.Models
{
    public class Questao
    {
        public int CodigoQuestao { get; set; }
        public int CodigoSimulado { get; set; }
        public int Numero { get; set; }
        public int CodigoPergunta { get; set; }
        public int CodigoRespostaUsada { get; set; }
        public List<int> CodigoRespostasUsadas { get; set; }
        public Pergunta Pergunta { get; set; }
        public List<Respondida> Respondidas { get; set; }
        public Simulado Simulado { get; set; }
        public int CodigoUsuario { get; set; }

        public int QuantidadeDeQuestoes { get; set; }

        public bool Acertou { get; set; }
        public override string ToString()
        {
            return $"CodigoQuestao: {CodigoQuestao} - CodigoSimulado: {CodigoSimulado} - Número: {Numero} - CodigoPergunta: {CodigoPergunta}";
        }
    }
}
