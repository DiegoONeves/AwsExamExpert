using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwsExamExpert.Models
{
    public class Pergunta
    {
        [Display(Name ="Código da Pergunta")]
        public int CodigoPergunta { get; set; }

        [Display(Name = "Prova")]
        public int CodigoProva { get; set; }
        public string Texto { get; set; }

        [Display(Name = "Múltipla Escolha")]
        public bool MultiplaEscolha { get; set; }
        public bool Ativo { get; set; }
        public List<Resposta> Respostas { get; set; }
        public Prova Prova { get; set; }

        [Display(Name = "Domínio")]
        public int CodigoDominio { get; set; }
        public Dominio Dominio { get; set; }

        public override string ToString()
        {
            return $"CodigoPergunta: {CodigoPergunta} - Pegunta: {Texto}";
        }
    }
}
