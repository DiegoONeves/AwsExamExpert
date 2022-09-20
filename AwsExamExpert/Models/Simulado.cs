using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AwsExamExpert.Models
{
    public class Simulado
    {
        public int CodigoSimulado { get; set; }
        public int CodigoUsuario { get; set; }
        [Display(Name = "Data")]
        public DateTime DataDoSimulado { get; set; }

        [Display(Name = "Nº de Questões")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(minimum: 1, maximum: 500, ErrorMessage = "O campo {0} deve ter seu valor entre {1} e {2}")]
        public int QuantidadeDeQuestoes { get; set; }
        public decimal PontosDisputados { get; set; }

        [Display(Name = "Pontos")]
        public decimal PontosConquistados { get; set; }
        public decimal Percentual { get; set; }

        [Display(Name = "Percentual de acerto")]
        public string PercentualFormatado { get; set; }

        public bool Aprovado { get; set; }
        public bool Finalizado { get; set; }

        [Display(Name = "Situação")]
        public string Situacao { get; set; }

        [Display(Name = "Tempo (em minutos)")]
        public int TempoDeProvaEmMinutos { get; set; }
        public List<Questao> Questoes { get; set; }
        [Display(Name = "Usuário")]
        public Usuario Usuario { get; set; }

        [Display(Name = "Prova")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int CodigoProva { get; set; }

        public Prova Prova { get; set; }
        public List<SelectListItem> Provas { get; set; }

        [Display(Name = "Média de Pontos")]
        public decimal PontuacaoMedia { get; set; }

        public List<Gabarito> Gabarito { get; set; } = new List<Gabarito>();

        public override string ToString()
        {
            return $"CodigoSimulado: {CodigoSimulado} - CodigoUsuario: {CodigoUsuario} - DataDoSimulado: {DataDoSimulado}";
        }
    }
}
