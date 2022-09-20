using System;

namespace AwsExamExpert.Models
{
    public class LogDeErro
    {
        public int CodigoLogDeErro { get; set; }
        public string Erro { get; set; }
        public string Acao { get; set; }
        public int? CodigoUsuario { get; set; }
        public DateTime DataHoraDoErro { get; set; }
    }
}
