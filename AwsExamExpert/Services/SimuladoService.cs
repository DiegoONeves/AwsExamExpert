using AwsExamExpert.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace AwsExamExpert
{
    public class SimuladoService : BaseService
    {
        private readonly UsuarioService _usuarioService;
        public SimuladoService(IConfiguration config, UsuarioService usuarioService)
            : base(config)
        {
            _usuarioService = usuarioService;
        }
        public void GerarNovoSimulado(Simulado novoSimulado)
        {
            novoSimulado.DataDoSimulado = DateTime.Now;
            novoSimulado.Finalizado = false;
            novoSimulado.Percentual = 0;
            novoSimulado.Aprovado = false;
            novoSimulado.PontosConquistados = 0;
            novoSimulado.PontosDisputados = 0;

            using var scope = new TransactionScope();
            using (var conn = new SqlConnection(ConnectionString))
                novoSimulado.CodigoSimulado = conn.ExecuteScalar<int>("insert into Simulado (DataDoSimulado,CodigoProva,CodigoUsuario,QuantidadeDeQuestoes,Finalizado,Percentual,Aprovado,PontosConquistados,PontosDisputados) values(@DataDoSimulado,@CodigoProva,@CodigoUsuario,@QuantidadeDeQuestoes,@Finalizado,@Percentual,@Aprovado,@PontosConquistados,@PontosDisputados); SELECT SCOPE_IDENTITY()", novoSimulado);

            GerarQuestoes(novoSimulado);

            novoSimulado.Questoes = ObterQuestao(codigoSimulado: novoSimulado.CodigoSimulado);

            foreach (var q in novoSimulado.Questoes)
                novoSimulado.PontosDisputados += q.Pergunta.Respostas.Select(x => x.Pontuacao).Sum();

            AtualizarPontuacaoDisputada(novoSimulado.CodigoSimulado, novoSimulado.PontosDisputados);
            AtualizarQuantidadeDeQuestoes(novoSimulado.CodigoSimulado);

            scope.Complete();
        }

        private void GerarQuestoes(Simulado simulado)
        {
            var perguntasUsadas = ObterPerguntasAleatorios(simulado);
            int numeroQuestao = 1;
            foreach (var p in perguntasUsadas)
            {
                var questao = new Questao { CodigoPergunta = p.CodigoPergunta, Numero = numeroQuestao, CodigoSimulado = simulado.CodigoSimulado };
                InserirQuestao(questao);
                numeroQuestao++;
                InserirOrdemResposta(simulado.CodigoSimulado, questao.CodigoQuestao, p.Respostas);
            }
        }

        private void InserirQuestao(Questao novaQuestao)
        {
            using var conn = new SqlConnection(ConnectionString);
            novaQuestao.CodigoQuestao = conn.ExecuteScalar<int>("insert into Questao (CodigoSimulado,Numero,CodigoPergunta) values(@CodigoSimulado,@Numero,@CodigoPergunta); SELECT SCOPE_IDENTITY()", novaQuestao);
        }

        public void ApurarResultadoParcial(int codigoSimulado)
        {
            var resultado = ObterSimuladosPor(codigoSimulado: codigoSimulado).FirstOrDefault();
            resultado.PontosConquistados = 0;

            List<Resposta> respostasCorretas = new();
            foreach (var q in resultado.Questoes.Take(50))
                respostasCorretas.AddRange(q.Pergunta.Respostas.Where(x => x.Correta));

            List<Respondida> respondidas = new();
            foreach (var q in resultado.Questoes.Where(x => x.Respondidas.Any()))
                respondidas.AddRange(q.Respondidas);

            int pontuacaoMaxima = 1000;
            decimal pontuacaoPorQuestao = (decimal)pontuacaoMaxima / (decimal)respostasCorretas.Count;
            int quantasAcertadas = 0;

            foreach (var r in respondidas)
                quantasAcertadas += respostasCorretas.Select(x => x.CodigoResposta).Contains(r.CodigoResposta) ? 1 : 0;

            resultado.PontosConquistados = (decimal)(quantasAcertadas * pontuacaoPorQuestao);
            resultado.Percentual = ((decimal)resultado.PontosConquistados) * 100 / pontuacaoMaxima;
            resultado.Aprovado = resultado.Percentual >= 72;
            resultado.Finalizado = resultado.Questoes.All(x => x.Respondidas.Count > 0);
            resultado.TempoDeProvaEmMinutos = (DateTime.Now - resultado.DataDoSimulado).Minutes;
            AtualizarPontuacaoConquistada(resultado);
        }

        public List<Simulado> ObterSimuladosPor(int? codigoSimulado = null, int? codigoUsuario = null, bool? finalizado = null)
        {
            var simulados = new List<Simulado>();
            using (var conn = new SqlConnection(ConnectionString))
                simulados = conn.Query<Simulado>("SELECT * FROM Simulado WHERE CodigoSimulado = ISNULL(@CodigoSimulado,CodigoSimulado) AND CodigoUsuario = ISNULL(@CodigoUsuario,CodigoUsuario) AND Finalizado = ISNULL(@Finalizado,Finalizado);", new { @CodigoSimulado = codigoSimulado, @CodigoUsuario = codigoUsuario, @Finalizado = finalizado }).ToList();

            foreach (var s in simulados)
            {
                s.Questoes = ObterQuestao(codigoSimulado: s.CodigoSimulado);
                s.Prova = ObterProvas(codigoProva: s.CodigoProva).FirstOrDefault();
                s.Usuario = _usuarioService.ObterUsuarioPor(codigoUsuario: s.CodigoUsuario).FirstOrDefault();
                s.Situacao = s.Aprovado ? "Aprovado" : "Reprovado";
                s.PercentualFormatado = s.Percentual.ToString() + "%";

                using var conn = new SqlConnection(ConnectionString);
                s.PontuacaoMedia = conn.ExecuteScalar<decimal>("SELECT AVG(PontosConquistados) FROM Simulado WHERE CodigoUsuario = @CodigoUsuario AND CodigoProva = @CodigoProva AND Finalizado = 1;", new { @CodigoUsuario = s.CodigoUsuario, @CodigoProva = s.CodigoProva });
                s.Gabarito = GerarGabarito(s.Questoes);
            }
            return simulados;
        }

        public void AtualizarQuantidadeDeQuestoes(int codigoSimulado)
        {
            int qtdDeQuestoes = ObterQuantidadeDeQuestoes(codigoSimulado);
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("update Simulado SET QuantidadeDeQuestoes = @QuantidadeDeQuestoes WHERE CodigoSimulado = @CodigoSimulado ", new { @QuantidadeDeQuestoes = qtdDeQuestoes, @CodigoSimulado = codigoSimulado });
        }

        public void AtualizarPontuacaoConquistada(Simulado simulado)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("update Simulado SET PontosConquistados = @PontosConquistados, Aprovado = @Aprovado, Percentual = @Percentual, TempoDeProvaEmMinutos = @TempoDeProvaEmMinutos, Finalizado = @Finalizado WHERE CodigoSimulado = @CodigoSimulado ", simulado);
        }

        public void AtualizarPontuacaoDisputada(int codigoSimulado, decimal pontuacaoDisputada)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("update Simulado SET PontosDisputados = @PontosDisputados WHERE CodigoSimulado = @CodigoSimulado ", new { @CodigoSimulado = codigoSimulado, @PontosDisputados = pontuacaoDisputada });
        }

        public (bool, string) ResponderQuestao(Questao questao)
        {
            questao.CodigoRespostasUsadas = new List<int>();
            if (questao.Pergunta.MultiplaEscolha)
            {
                foreach (var item in questao.Pergunta.Respostas.Where(x => x.Respondida))
                    questao.CodigoRespostasUsadas.Add(item.CodigoResposta);
            }
            else
                questao.CodigoRespostasUsadas.Add(questao.CodigoRespostaUsada);

            using var tran = new TransactionScope();
            RemoverRespondidas(questao.CodigoQuestao);

            if (questao.CodigoRespostasUsadas.Count == 0)
            {
                tran.Complete();
                return (false, "Nenhuma resposta fornecida");
            }
            if (questao.Pergunta.MultiplaEscolha)
            {
                int qtdRespostasCorretas = ObterRepostas(codigoPergunta: questao.CodigoPergunta).Count(x => x.Correta);
                if (questao.CodigoRespostasUsadas.Count != qtdRespostasCorretas)
                {
                    tran.Complete();
                    return (false, $"Você deve selecioner {qtdRespostasCorretas} respostas");
                }
            }
            foreach (var codigoResposta in questao.CodigoRespostasUsadas)
                InserirRespondida(questao.CodigoQuestao, codigoResposta);

            ApurarResultadoParcial(questao.CodigoSimulado);
            tran.Complete();

            return (true, "Sucesso");

        }

        public List<Questao> ObterQuestao(int codigoSimulado, int? numero = null, int? codigoUsuario = null)
        {
            List<Questao> questoes = new();

            using (var conn = new SqlConnection(ConnectionString))
                questoes = conn.Query<Questao>("SELECT * FROM Questao WHERE CodigoSimulado = @CodigoSimulado AND Numero = ISNULL(@Numero,Numero);", new { @CodigoSimulado = codigoSimulado, @Numero = numero }).ToList();

            foreach (var q in questoes)
            {
                var ordemResposta = ObterOrdemRespostaPor(codigoSimulado: codigoSimulado, codigoPergunta: q.CodigoPergunta, codigoQuestao: q.CodigoQuestao);
                q.QuantidadeDeQuestoes = ObterQuantidadeDeQuestoes(q.CodigoSimulado);
                q.Pergunta = ObterPerguntasPor(codigoPergunta: q.CodigoPergunta).FirstOrDefault();

                foreach (var r in q.Pergunta.Respostas)
                    r.Ordem = ordemResposta.FirstOrDefault(x => x.CodigoResposta == r.CodigoResposta).Ordem;

                q.Pergunta.Respostas = q.Pergunta.Respostas.OrderBy(x => x.Ordem).ToList();

                q.Respondidas = ObterRespondidasPor(codigoQuestao: q.CodigoQuestao);
                if (q.Respondidas?.Count > 0)
                {
                    if (q.Pergunta.MultiplaEscolha)
                    {
                        foreach (var r in q.Respondidas)
                            q.Pergunta.Respostas.FirstOrDefault(x => x.CodigoResposta == r.CodigoResposta).Respondida = true;
                    }
                    else
                        q.CodigoRespostaUsada = q.Respondidas.FirstOrDefault().CodigoResposta;
                }
            }
            return questoes;
        }



        public List<Pergunta> ObterPerguntasPor(int? codigoPergunta = null, int? codigoProva = null)
        {
            List<Pergunta> perguntas = new();
            using (var conn = new SqlConnection(ConnectionString))
            {
                perguntas = conn.Query<Pergunta>("SELECT * FROM Pergunta WHERE CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta) AND CodigoProva = ISNULL(@CodigoProva,CodigoProva)", new { @CodigoPergunta = codigoPergunta, @CodigoProva = codigoProva }).ToList();
                foreach (var p in perguntas)
                {
                    p.Respostas = ObterRepostas(p.CodigoPergunta);
                    p.Prova = ObterProvas(codigoProva: p.CodigoProva).FirstOrDefault();
                    p.Dominio = ObterDominiosPor(codigoDominio: p.CodigoDominio, codigoProva: p.CodigoProva).FirstOrDefault();
                }
            }
            return perguntas;
        }

        private List<Pergunta> ObterPerguntasAleatorios(Simulado simulado)
        {
            var rnd = new Random();
            return ObterPerguntasPor(codigoProva: simulado.CodigoProva).OrderBy(item => rnd.Next()).Take(simulado.QuantidadeDeQuestoes).ToList();
        }

        private List<Resposta> ObterRepostas(int codigoPergunta)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Resposta>("SELECT * FROM Resposta WHERE CodigoPergunta = @CodigoPergunta", new { @CodigoPergunta = codigoPergunta }).ToList();
        }

        public List<Prova> ObterProvas(int? codigoProva = null, bool? ativo = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Prova>("SELECT * FROM Prova WHERE CodigoProva = ISNULL(@CodigoProva,CodigoProva) AND Ativo = ISNULL(@Ativo,Ativo)", new { @CodigoProva = codigoProva, @Ativo = ativo }).ToList();
        }

        public void RemoverRespondidas(int codigoQuestao)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("delete from Respondida where CodigoQuestao = @CodigoQuestao", new { @CodigoQuestao = codigoQuestao });
        }
        public Respondida InserirRespondida(int codigoQuestao, int codigoResposta)
        {
            Respondida respondida = new()
            {
                CodigoQuestao = codigoQuestao,
                CodigoResposta = codigoResposta
            };
            using (var conn = new SqlConnection(ConnectionString))
                conn.Execute("insert into Respondida (CodigoQuestao,CodigoResposta) values(@CodigoQuestao,@CodigoResposta)", respondida);

            return respondida;
        }

        public List<Respondida> ObterRespondidasPor(int? codigoQuestao = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Respondida>("SELECT * FROM Respondida WHERE CodigoQuestao = ISNULL(@CodigoQuestao,CodigoQuestao)", new { @CodigoQuestao = codigoQuestao }).ToList();
        }

        public void CancelarResultadoProva(int codigoSimulado, int? codigoUsuario = null)
        {
            var simuladoEmAndamento = ObterSimuladosPor(codigoSimulado: codigoSimulado, codigoUsuario: codigoUsuario).FirstOrDefault();
            var questoes = simuladoEmAndamento.Questoes;
            using var trab = new TransactionScope();
            foreach (var q in questoes)
            {
                RemoverRespondidas(q.CodigoQuestao);
                DeletarOrdemRespostaPor(codigoSimulado: codigoSimulado, codigoPergunta: q.CodigoPergunta, codigoQuestao: q.CodigoQuestao);
                RemoverQuestoes(codigoSimulado: codigoSimulado, codigoQuestao: q.CodigoQuestao);
            }
            RemoverSimulado(codigoSimulado);
            trab.Complete();
        }

        public void RemoverAnotacao(int? codigoAnotacao = null, int? codigoPergunta = null, int? codigoUsuario = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("delete from Anotacao where CodigoAnotacao = ISNULL(@CodigoAnotacao,CodigoAnotacao) AND CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta) AND CodigoUsuario = ISNULL(@CodigoUsuario,CodigoUsuario)",
                new
                {
                    @CodigoAnotacao = codigoAnotacao,
                    @CodigoPergunta = codigoPergunta,
                    @CodigoUsuario = codigoUsuario
                });
        }
        public void RemoverSimulado(int? codigoSimulado = null, int? codigoUsuario = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("delete from Simulado where CodigoSimulado = ISNULL(@CodigoSimulado,CodigoSimulado) AND CodigoUsuario = ISNULL(@CodigoUsuario,CodigoUsuario);", new { @CodigoSimulado = codigoSimulado, @CodigoUsuario = codigoUsuario });
        }
        public void RemoverQuestoes(int? codigoSimulado = null, int? codigoQuestao = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("delete from Questao where CodigoSimulado = ISNULL(@CodigoSimulado,CodigoSimulado) AND CodigoQuestao = ISNULL(@CodigoQuestao,CodigoQuestao);", new { @CodigoSimulado = codigoSimulado, @CodigoQuestao = codigoQuestao });
        }

        public int ObterQuantidadeDePerguntas(int codigoProva)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.ExecuteScalar<int>("select count(*) from Pergunta where CodigoProva = @CodigoProva AND Ativo = 1;", new { @CodigoProva = codigoProva });
        }

        public int ObterQuantidadeDeQuestoes(int codigoSimulado)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.ExecuteScalar<int>("select count(*) from Questao where CodigoSimulado = @CodigoSimulado;", new { @CodigoSimulado = codigoSimulado });
        }

        public List<Dominio> ObterDominiosPor(int? codigoDominio = null, int? codigoProva = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Dominio>("select * from Dominio where CodigoDominio = ISNULL(@CodigoDominio,CodigoDominio) AND CodigoProva = ISNULL(@CodigoProva,CodigoProva)", new { @CodigoDominio = codigoDominio, @CodigoProva = codigoProva }).ToList();
        }

        public List<Gabarito> GerarGabarito(List<Questao> questoes)
        {
            List<Gabarito> gabarito = new();
            var respondidas = ObterRespondidasPor();
            foreach (var q in questoes)
            {
                Gabarito g = new();
                g.NumeroQuestao = q.Numero;
                g.Texto = q.Pergunta.Texto;
                g.Resposta = string.Join(" | ", q.Pergunta.Respostas.Where(x => x.Correta).Select(x => x.Texto));
                g.Respondida = string.Join(" | ", q.Pergunta.Respostas.Where(y => q.Respondidas.Select(x => x.CodigoResposta).Contains(y.CodigoResposta)).Select(x => x.Texto));
                g.Resultado = q.Pergunta.Respostas.Where(x => x.Correta).Select(x => x.CodigoResposta).All(z => q.Respondidas.Select(x => x.CodigoResposta).Contains(z)) ? "Correta" : "Incorreta";
                gabarito.Add(g);
            }

            return gabarito;
        }

        public void InserirOrdemResposta(int codigoSimulado, int codigoQuestao, List<Resposta> respostas)
        {
            var rnd = new Random();
            var listaEmbaralhada = respostas.OrderBy(item => rnd.Next()).ToList();
            List<string> letras = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            for (int i = 0; i < listaEmbaralhada.Count; i++)
                listaEmbaralhada[i].Ordem = letras[i];

            foreach (var item in listaEmbaralhada)
            {
                var ordemResposta = new OrdemResposta
                {
                    CodigoSimulado = codigoSimulado,
                    CodigoPergunta = item.CodigoPergunta,
                    CodigoResposta = item.CodigoResposta,
                    CodigoQuestao = codigoQuestao,
                    Ordem = item.Ordem
                };
                using var conn = new SqlConnection(ConnectionString);
                conn.Execute("insert into OrdemResposta (CodigoSimulado,CodigoPergunta,CodigoQuestao,CodigoResposta,Ordem) values(@CodigoSimulado,@CodigoPergunta,@CodigoQuestao,@CodigoResposta,@Ordem)", ordemResposta);
            }

        }

        public List<OrdemResposta> ObterOrdemRespostaPor(int? codigoSimulado = null, int? codigoPergunta = null, int? codigoQuestao = null, int? codigoResposta = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<OrdemResposta>("select * from OrdemResposta where CodigoSimulado = ISNULL(@CodigoSimulado,CodigoSimulado) AND CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta) AND CodigoQuestao = ISNULL(@CodigoQuestao,CodigoQuestao) AND CodigoResposta = ISNULL(@CodigoResposta,CodigoResposta)",
                new
                {
                    @CodigoQuestao = codigoQuestao,
                    @CodigoResposta = codigoResposta,
                    @CodigoSimulado = codigoSimulado,
                    @CodigoPergunta = codigoPergunta
                }).ToList();
        }

        public List<OrdemResposta> DeletarOrdemRespostaPor(int? codigoSimulado = null, int? codigoPergunta = null, int? codigoQuestao = null, int? codigoResposta = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<OrdemResposta>("delete from OrdemResposta where CodigoSimulado = ISNULL(@CodigoSimulado,CodigoSimulado) AND CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta) AND CodigoQuestao = ISNULL(@CodigoQuestao,CodigoQuestao) AND CodigoResposta = ISNULL(@CodigoResposta,CodigoResposta)",
                new
                {
                    @CodigoQuestao = codigoQuestao,
                    @CodigoResposta = codigoResposta,
                    @CodigoSimulado = codigoSimulado,
                    @CodigoPergunta = codigoPergunta
                }).ToList();
        }

    }
}
