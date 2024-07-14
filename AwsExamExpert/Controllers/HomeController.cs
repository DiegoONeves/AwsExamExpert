using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AwsExamExpert.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace AwsExamExpert.Controllers
{

    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        UsuarioService _service;
        public HomeController(LogService logService, ILogger<HomeController> logger, UsuarioService service)
            : base(logService)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_service.VerificarAcesso(login.WhatsApp))
                {
                    var usuarioDoBanco = _service.ObterUsuarioPor(WhatsApp: login.WhatsApp, liberado: true).FirstOrDefault();

                    if (usuarioDoBanco != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuarioDoBanco.Nome),
                            new Claim(ClaimTypes.MobilePhone, usuarioDoBanco.WhatsApp),
                            new Claim(ClaimTypes.Role, usuarioDoBanco.Administrador? "Administrador":"Comum"),
                            new Claim(ClaimTypes.NameIdentifier, usuarioDoBanco.CodigoUsuario.ToString())
                        };

                        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var userPrincipal = new ClaimsPrincipal(userIdentity);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal).Wait();
                        ExibirMensagemFrontEnd(CallBackMessageType.Success, $"Bem-vindo, {usuarioDoBanco.Nome}!");
                        return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                    }
                }

                ModelState.AddModelError("", $"Acesso não permitido, entre em contato no WhatsApp {_service.WhatsAppAdministrador}");
            }
            return View(login);
        }
        [HttpGet]
        public IActionResult Sair()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            //construir mecanismo que mostra usuário logado
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
