using AwsExamExpert.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace AwsExamExpert.Controllers
{
    public class BaseController: Controller
    {
        LogService _service;
        public BaseController(LogService service)
        {
            _service = service;
        }
        protected int ObterCodigoUsuarioLogado() => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        protected void ExibirMensagemFrontEnd(CallBackMessageType tipoMensagemCallBack, string mensagem)
        {
            TempData[tipoMensagemCallBack.ToString()] = mensagem;
        }

        protected void GravarLogErro(string exception, string acao) 
        {
            _service.GravarLogDeErro(new LogDeErro
            {
                Erro = exception,
                Acao = acao,
                CodigoUsuario = User.Identity.IsAuthenticated? ObterCodigoUsuarioLogado(): null,
                DataHoraDoErro = DateTime.Now
            });
        }
    }

    public enum CallBackMessageType
    {
        Success = 1,
        Warning = 2,
        Error = 3,
        Info = 4
    }
}
