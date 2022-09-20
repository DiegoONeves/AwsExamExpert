namespace AwsExamExpert.Models
{
    public class Prova
    {
        public int CodigoProva { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int QuantidadeDeQuestoes { get; set; }

        public override string ToString()
        {
            return $"CodigoProva: {CodigoProva} - Descrição: {Descricao} - Ativo: {Ativo}";
        }
    }
}
