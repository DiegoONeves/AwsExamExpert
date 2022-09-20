using System.ComponentModel.DataAnnotations;

namespace AwsExamExpert.Models
{
    public class LoginViewModel
    {
        [Display(Name = "WhatsApp")]
        [StringLengthAttribute(11, ErrorMessage = "O campo {0} é obrigatório deve ter {1} caracteres")]
        public string WhatsApp { get; set; }

        public override string ToString()
        {
            return $"WhatsApp: {WhatsApp}";
        }
    }
}
