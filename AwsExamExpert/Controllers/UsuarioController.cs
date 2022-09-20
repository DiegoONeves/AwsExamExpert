using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AwsExamExpert.Models;
using System;
using System.Linq;

namespace AwsExamExpert.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;

        UsuarioService _service;
        public UsuarioController(ILogger<UsuarioController> logger, UsuarioService service, LogService logService)
            : base(logService)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            if (!_service.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                return RedirectToAction("Index", "Home");

            return View(_service.ObterUsuarioPor());
        }


        public IActionResult Editar()
        {
            return View(_service.ObterUsuarioPor(codigoUsuario: ObterCodigoUsuarioLogado()).FirstOrDefault());
        }

        [Route("Editar/{codigoUsuario}")]
        [HttpGet]
        public IActionResult Editar([FromRoute]int codigoUsuario)
        {
            if (!_service.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                return RedirectToAction("Index", "Home");

            return View(_service.ObterUsuarioPor(codigoUsuario: codigoUsuario).FirstOrDefault());
        }


        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            return AtualizarUsuario(usuario);
        }

        [HttpPost]
        [Route("Editar/{codigoUsuario}")]
        public IActionResult Editar([FromRoute] int codigoUsuario, Usuario usuario)
        {
            return AtualizarUsuario(usuario);
        }

        public IActionResult AtualizarUsuario(Usuario usuario) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioLogado = _service.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault();
                    usuario.Nome = usuario.Nome;
                    usuario.WhatsApp = usuario.WhatsApp;

                    if (usuarioLogado.Administrador)
                    {
                        usuario.Liberado = usuario.Liberado;
                        usuario.RenovarCadastro = usuario.RenovarCadastro;
                    }
                    else
                    {
                        usuario.RenovarCadastro = false;
                        usuario.Administrador = false;
                    }

                    _service.AtualizarUsuario(usuario);

                    ExibirMensagemFrontEnd(CallBackMessageType.Success, "Cadastro atualizado com sucesso!");
                    return RedirectToAction("Orientacao");
                }
                catch (Exception ex)
                {
                    GravarLogErro(ex.Message, "POST:Edição de Usuário");
                }

            }

            return View(usuario);
        }
        [AllowAnonymous]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.CadastrarUsuario(usuario);
                    ExibirMensagemFrontEnd(CallBackMessageType.Success, "Cadastro efetuado com sucesso!");
                    return RedirectToAction("Orientacao");
                }
                catch (Exception ex)
                {
                    GravarLogErro(ex.Message, "POST:Cadastro de Usuário");
                }

            }

            return View(usuario);
        }

        [AllowAnonymous]
        public IActionResult Orientacao()
        {
            return View();
        }
    }
}
