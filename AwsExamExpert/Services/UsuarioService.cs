using Dapper;
using Microsoft.Extensions.Configuration;
using AwsExamExpert.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AwsExamExpert
{
    public class UsuarioService : BaseService
    {
        public UsuarioService(IConfiguration config)
    : base(config)
        {
        }
        public void CadastrarUsuario(Usuario usuario)
        {
            usuario.DataDeCadastro = DateTime.Now;
            usuario.DataDeVencimento = DateTime.Now.AddMonths(6);
            using (var conn = new SqlConnection(ConnectionString)) 
            usuario.CodigoUsuario = conn.ExecuteScalar<int>("insert into Usuario (Nome,WhatsApp,Administrador,Liberado,DataDeCadastro,DataDeVencimento) values(@Nome,@WhatsApp,@Administrador,@Liberado,@DataDeCadastro,@DataDeVencimento); SELECT SCOPE_IDENTITY()", usuario);
        }
        public void AtualizarUsuario(Usuario usuario)
        {
            if (usuario.RenovarCadastro)
                usuario.DataDeVencimento = DateTime.Now.AddMonths(6);
            else
                usuario.DataDeVencimento = null;

            using (var conn = new SqlConnection(ConnectionString))
            conn.Execute("update Usuario set Nome = @Nome, WhatsApp = @WhatsApp, Administrador = ISNULL(@Administrador,Administrador), Liberado = ISNULL(@Liberado,Liberado), DataDeVencimento = ISNULL(@DataDeVencimento,DataDeVencimento) WHERE CodigoUsuario = @CodigoUsuario;", usuario);
        }
        public List<Usuario> ObterUsuarioPor(int? codigoUsuario = null, string WhatsApp = null, bool? liberado = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Usuario>("select * from Usuario where CodigoUsuario = isnull(@CodigoUsuario,CodigoUsuario) and WhatsApp = ISNULL(@WhatsApp,WhatsApp) AND Liberado = ISNULL(@Liberado,Liberado);", new { CodigoUsuario = codigoUsuario, @WhatsApp = WhatsApp, @Liberado = liberado }).ToList();

        }
        public bool VerificarAcesso(string WhatsApp) => ObterUsuarioPor(WhatsApp: WhatsApp, liberado: true).Any();

        private void VerificarWhatsApp() 
        {

        }
    }
}
