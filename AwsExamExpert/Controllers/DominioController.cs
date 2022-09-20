using AwsExamExpert.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AwsExamExpert.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DominioController : BaseController
    {
        private SimuladoService _simuladorService;
        private UsuarioService _usuarioService;
        public DominioController(LogService logService, SimuladoService simuladorService, UsuarioService usuarioService) 
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
        public IActionResult Cadastrar(Dominio dominio)
        {
            return View();
        }

        public IActionResult Editar(int codigoDominio)
        {
            if (!_usuarioService.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                return RedirectToAction("Index", "Home");

            var pergunta = _simuladorService.ObterDominiosPor(codigoDominio: codigoDominio).FirstOrDefault();

            return View(pergunta);
        }

        [HttpPost]
        public IActionResult Editar(Dominio dominio)
        {
            return View(dominio);
        }
    }
}
