using System;
using System.ComponentModel.DataAnnotations;

namespace AwsExamExpert.Models
{
    public class Usuario
    {
        public int CodigoUsuario { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(length: 50, ErrorMessage = "O campo {0} é obrigatório pode ter no máximo {1} caracteres")]
        public string Nome { get; set; }

        [Display(Name ="WhatsApp")]
        [StringLengthAttribute(11, ErrorMessage = "O campo {0} é obrigatório deve ter {1} caracteres")]
        public string WhatsApp { get; set; }
        public bool Administrador { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataDeCadastro { get; set; }
        public bool Liberado { get; set; }

        [Display(Name = "Renovar cadastro?")]
        public bool RenovarCadastro { get; set; }

        [Display(Name = "Data de Vencimento")]
        public DateTime? DataDeVencimento { get; set; }

        public override string ToString()
        {
            return $"CodigoUsuario: {CodigoUsuario} - Nome: {Nome} - WhatsApp: {WhatsApp} - Administrador: {Administrador} - Liberado: {Liberado} - DataDeCadastro: {DataDeCadastro} - DataDeVencimento: {DataDeVencimento}";
        }

    }
}
