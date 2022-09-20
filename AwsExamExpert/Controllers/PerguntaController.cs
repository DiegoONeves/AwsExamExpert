using AwsExamExpert.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AwsExamExpert.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PerguntaController : BaseController
    {
        private SimuladoService _simuladorService;
        private UsuarioService _usuarioService;
        public PerguntaController(LogService logService, SimuladoService simuladorService, UsuarioService usuarioService) 
            : base(logService)
        {
            _simuladorService = simuladorService;
            _usuarioService = usuarioService;
        }
        public IActionResult Index()
        {
            return View(_simuladorService.ObterPerguntasPor());
        }

        public IActionResult Cadastrar()
        {
            var novaPergunta = new Pergunta
            {
                Respostas = new List<Resposta>()
            };
            novaPergunta.Respostas.AddRange(new List<Resposta> 
            {
                new Resposta { Texto = "Resposta 1" },
                new Resposta { Texto = "Resposta 2" },
                new Resposta { Texto = "Resposta 3" },
                new Resposta { Texto = "Resposta 4" }
            });
            return View(novaPergunta);
        }

        [HttpPost]
        public IActionResult Cadastrar(Pergunta pergunta)
        {
            return View(pergunta);
        }

        public IActionResult Editar(int codigoPergunta)
        {
            if (!_usuarioService.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                return RedirectToAction("Index", "Home");

            var pergunta = _simuladorService.ObterPerguntasPor(codigoPergunta: codigoPergunta).FirstOrDefault();

            return View(pergunta);
        }

        [HttpPost]
        public IActionResult Editar(Pergunta pergunta)
        {
            return View(pergunta);
        }
    }
}
