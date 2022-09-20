using AwsExamExpert.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AwsExamExpert.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ProvaController : BaseController
    {
        private SimuladoService _simuladorService;
        private UsuarioService _usuarioService;
        public ProvaController(LogService logService, SimuladoService simuladorService, UsuarioService usuarioService) 
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
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Prova prova)
        {
            return View(prova);
        }

        public IActionResult Editar(int codigoProva)
        {
            if (!_usuarioService.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                return RedirectToAction("Index", "Home");

            var pergunta = _simuladorService.ObterProvas(codigoProva: codigoProva).FirstOrDefault();

            return View(pergunta);
        }

        [HttpPost]
        public IActionResult Editar(Prova prova)
        {
            return View(prova);
        }
    }
}
