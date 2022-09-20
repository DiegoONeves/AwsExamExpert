using System.Collections.Generic;

namespace AwsExamExpert.Models
{
    public class Dominio
    {
        public int CodigoDominio { get; set; }
        public string Descricao { get; set; }
        public int CodigoProva { get; set; }
        public Prova Prova { get; set; }
        public List<Pergunta> Perguntas { get; set; }
    }
}
