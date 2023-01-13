using System.ComponentModel.DataAnnotations;

namespace AwsExamExpert.Models
{
    public class RefazerProvaViewModel
    {
        public int CodigoUsuario { get; set; }
        public string Prova { get; set; }
        public int CodigoSimulado { get; set; }
        [Display(Name = "Somente Incorretas")]
        public bool SomenteIncorretas { get; set; }

    }
}
