using AwsExamExpert.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace AwsExamExpert
{
    public class LogService: BaseService
    {
        public LogService(IConfiguration config)
    : base(config)
        {
        }


        public void GravarLogDeErro(LogDeErro logDeErro) 
        {
            logDeErro.DataHoraDoErro = DateTime.Now;
            using (var conn = new SqlConnection(ConnectionString))
            {         
                logDeErro.CodigoLogDeErro = conn.ExecuteScalar<int>("insert into LogDeErro (CodigoUsuario,Erro,Acao,DataHoraDoErro) values(@CodigoUsuario,@Erro,@Acao,@DataHoraDoErro); SELECT SCOPE_IDENTITY()", logDeErro);             
            }
        }
    }
}
