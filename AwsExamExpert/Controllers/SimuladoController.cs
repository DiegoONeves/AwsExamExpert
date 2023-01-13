using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AwsExamExpert.Models;
using System.Linq;

namespace AwsExamExpert.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class SimuladoController : BaseController
    {
        SimuladoService _service;
        UsuarioService _usuarioService;

        public SimuladoController(LogService logService, SimuladoService service, UsuarioService usuarioService)
            : base(logService)
        {
            _service = service;
            _usuarioService = usuarioService;
        }
        public IActionResult Resultados()
        {
            if (_usuarioService.ObterUsuarioPor(ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                return View(_service.ObterSimuladosPor(finalizado: true).OrderByDescending(x => x.DataDoSimulado));

            return View(_service.ObterSimuladosPor(codigoUsuario: ObterCodigoUsuarioLogado(), finalizado: true).OrderByDescending(x => x.DataDoSimulado));
        }

        public IActionResult NovaProva()
        {
            return View(new Simulado
            {
                QuantidadeDeQuestoes = 0,
                Provas = _service.ObterProvas(ativo: true).Select(c => new SelectListItem()
                {
                    Text = $"{c.Descricao} ({_service.ObterQuantidadeDePerguntas(c.CodigoProva)})",
                    Value = c.CodigoProva.ToString()
                }).ToList(),
                CodigoUsuario = ObterCodigoUsuarioLogado()
            });
        }

        [HttpPost]
        public IActionResult Iniciar(Simulado novoSimulado)
        {
            if (ModelState.IsValid)
            {
                _service.GerarNovoSimulado(novoSimulado);
                return RedirectToAction("Pergunta", new { codigoSimulado = novoSimulado.CodigoSimulado, numeroQuestao = 1 });
            }

            ModelState.AddModelError("", "Verifique se os campos obrigatórios foram preenchidos.");
            return RedirectToAction("NovaProva");
        }

        [Route("Simulado/{codigoSimulado}/Questao/{numeroQuestao}")]
        [HttpGet]
        public IActionResult Pergunta([FromRoute] int codigoSimulado, [FromRoute] int numeroQuestao)
        {
            string mensagemModelError = TempData["MensagemModelError"]?.ToString();
            if (!string.IsNullOrEmpty(mensagemModelError))
                ModelState.AddModelError("", mensagemModelError);
            return View(_service.ObterQuestao(codigoSimulado, numeroQuestao, codigoUsuario: ObterCodigoUsuarioLogado()).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Responder(Questao questao)
        {
            questao.CodigoUsuario = ObterCodigoUsuarioLogado();
            var resultado = _service.ResponderQuestao(questao);
            if (!resultado.Item1)
            {
                TempData["MensagemModelError"] = resultado.Item2;
                return RedirectToAction("Pergunta", new { codigoSimulado = questao.CodigoSimulado, numeroQuestao = questao.Numero });
            }

            var simuladoAtual = _service.ObterSimuladosPor(codigoSimulado: questao.CodigoSimulado).FirstOrDefault();
            if (simuladoAtual.Finalizado)
                return RedirectToAction("Resultado", new { codigoSimulado = questao.CodigoSimulado });

            return RedirectToAction("Pergunta", new { codigoSimulado = questao.CodigoSimulado, numeroQuestao = ++questao.Numero });
        }


        [Route("Simulado/{codigoSimulado}/Resultado")]
        public IActionResult Resultado([FromRoute] int codigoSimulado)
        {
            return View(_usuarioService.ObterUsuarioPor(codigoUsuario: ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador ?
                _service.ObterSimuladosPor(codigoSimulado: codigoSimulado).FirstOrDefault()
                : _service.ObterSimuladosPor(codigoSimulado: codigoSimulado, codigoUsuario: ObterCodigoUsuarioLogado()));
        }

        public IActionResult Excluir(int codigoSimulado)
        {
            if (_usuarioService.ObterUsuarioPor(codigoUsuario: ObterCodigoUsuarioLogado()).FirstOrDefault().Administrador)
                _service.CancelarResultadoProva(codigoSimulado: codigoSimulado);
            else
                _service.CancelarResultadoProva(codigoSimulado: codigoSimulado, codigoUsuario: ObterCodigoUsuarioLogado());

            return RedirectToAction("Resultados");
        }

        public IActionResult RefazerProva(int codigoSimulado)
        {
            var simulado = _service.ObterSimuladosPor(codigoSimulado: codigoSimulado).FirstOrDefault();

            RefazerProvaViewModel model = new()
            {
                CodigoSimulado = codigoSimulado,
                Prova = simulado.Prova.Descricao
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult RefazerProva(RefazerProvaViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.CodigoUsuario = ObterCodigoUsuarioLogado();
                var novoSimulado = _service.RefazerSimulado(vm);
                if (novoSimulado != null)
                    return RedirectToAction("Pergunta", new { codigoSimulado = novoSimulado.CodigoSimulado, numeroQuestao = 1 });
            }

            ModelState.AddModelError("", "Verifique se os campos obrigatórios foram preenchidos.");
            return View(vm);
        }
    }
}
