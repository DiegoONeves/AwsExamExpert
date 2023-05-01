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
            novoSimulado.PontosDisputados = 1000;

            using var scope = new TransactionScope();
            using (var conn = new SqlConnection(ConnectionString))
                novoSimulado.CodigoSimulado = conn.ExecuteScalar<int>("insert into Simulado (DataDoSimulado,CodigoProva,CodigoUsuario,QuantidadeDeQuestoes,Finalizado,Percentual,Aprovado,PontosConquistados,PontosDisputados) values(@DataDoSimulado,@CodigoProva,@CodigoUsuario,@QuantidadeDeQuestoes,@Finalizado,@Percentual,@Aprovado,@PontosConquistados,@PontosDisputados); SELECT SCOPE_IDENTITY()", novoSimulado);

            GerarQuestoes(novoSimulado, ObterPerguntasAleatorios(novoSimulado, ativo: true));
            ElegerQuestoesQueSeraoPontuadas(novoSimulado.CodigoSimulado);

            novoSimulado.Questoes = ObterQuestao(codigoSimulado: novoSimulado.CodigoSimulado);

            AtualizarPontuacaoDisputada(novoSimulado.CodigoSimulado, novoSimulado.PontosDisputados);
            AtualizarQuantidadeDeQuestoes(novoSimulado.CodigoSimulado);

            scope.Complete();
        }

        public Simulado RefazerSimulado(RefazerProvaViewModel vm)
        {
            Simulado simuladoAntigo = ObterSimuladosPor(codigoSimulado: vm.CodigoSimulado).FirstOrDefault();

            var questoesUsadas = ObterQuestao(codigoSimulado: simuladoAntigo.CodigoSimulado, somenteIncorretas: vm.SomenteIncorretas);
            if (questoesUsadas is null || questoesUsadas.Count is 0)
                return null;
            var perguntas = ObterPerguntasPor(codigoPerguntaIn: questoesUsadas.Select(x => x.CodigoPergunta).ToList());

            Simulado novoSimulado = new()
            {
                CodigoUsuario = vm.CodigoUsuario,
                CodigoProva = simuladoAntigo.CodigoProva,
                QuantidadeDeQuestoes = questoesUsadas.Count,
                DataDoSimulado = DateTime.Now,
                Finalizado = false,
                Percentual = 0,
                Aprovado = false,
                PontosConquistados = 0,
                PontosDisputados = simuladoAntigo.PontosDisputados,
            };

            using var scope = new TransactionScope();
            using (var conn = new SqlConnection(ConnectionString))
                novoSimulado.CodigoSimulado = conn.ExecuteScalar<int>("insert into Simulado (DataDoSimulado,CodigoProva,CodigoUsuario,QuantidadeDeQuestoes,Finalizado,Percentual,Aprovado,PontosConquistados,PontosDisputados) values(@DataDoSimulado,@CodigoProva,@CodigoUsuario,@QuantidadeDeQuestoes,@Finalizado,@Percentual,@Aprovado,@PontosConquistados,@PontosDisputados); SELECT SCOPE_IDENTITY()", novoSimulado);

            GerarQuestoes(novoSimulado, perguntas);

            novoSimulado.Questoes = ObterQuestao(codigoSimulado: novoSimulado.CodigoSimulado);



            ElegerQuestoesQueSeraoPontuadas(novoSimulado.CodigoSimulado);


            AtualizarQuantidadeDeQuestoes(novoSimulado.CodigoSimulado);

            scope.Complete();

            return novoSimulado;
        }

        private void GerarQuestoes(Simulado novoSimulado, List<Pergunta> perguntasUsadas)
        {
            int numeroQuestao = 1;
            foreach (var p in perguntasUsadas)
            {
                var questao = new Questao { CodigoPergunta = p.CodigoPergunta, Numero = numeroQuestao, CodigoSimulado = novoSimulado.CodigoSimulado, UsadaNaPontuacao = false };
                InserirQuestao(questao);
                InserirAnotacao(new Anotacao { CodigoPergunta = p.CodigoPergunta, CodigoUsuario = novoSimulado.CodigoUsuario, Texto = "" });
                numeroQuestao++;
                InserirOrdemResposta(novoSimulado.CodigoSimulado, questao.CodigoQuestao, p.Respostas);
            }
        }

        private void ElegerQuestoesQueSeraoPontuadas(int codigoSimulado)
        {
            var questoesDoSimulado = ObterQuestao(codigoSimulado);
            int numeroDeQuestoes = questoesDoSimulado.Count;
            int numeroDeQuestoesQueSeraoUsadas = (int)(numeroDeQuestoes * 78) / 100;//neste exame 22% das questões são descartadas

            var rnd = new Random();
            var questoesUsadas = questoesDoSimulado.OrderBy(item => rnd.Next()).Take(numeroDeQuestoesQueSeraoUsadas).ToList();
            foreach (var q in questoesUsadas)
            {
                q.UsadaNaPontuacao = true;
                AtualizarQuestao(q);
            }

        }

        private void InserirQuestao(Questao novaQuestao)
        {
            using var conn = new SqlConnection(ConnectionString);
            novaQuestao.CodigoQuestao = conn.ExecuteScalar<int>("insert into Questao (CodigoSimulado,Numero,CodigoPergunta,UsadaNaPontuacao) values(@CodigoSimulado,@Numero,@CodigoPergunta,@UsadaNaPontuacao); SELECT SCOPE_IDENTITY();", novaQuestao);
        }

        public void ApurarResultadoParcial(int codigoSimulado)
        {
            var simulado = ObterSimuladosPor(codigoSimulado: codigoSimulado).FirstOrDefault();
            simulado.PontosConquistados = 0;

            var todasQuestoes = simulado.Questoes.ToList();
            List<Resposta> respostasCorretas = new();
            int respostasPontuaveis = 0;
            foreach (var q in todasQuestoes)
            {
                if (q.UsadaNaPontuacao)
                    respostasPontuaveis += q.Pergunta.Respostas.Count(x => x.Correta);

                respostasCorretas.AddRange(q.Pergunta.Respostas.Where(x => x.Correta));
            }
            List<Respondida> respondidas = new();
            foreach (var q in simulado.Questoes.Where(x => x.Respondidas.Any()))
                respondidas.AddRange(q.Respondidas);

            int pontuacaoMaxima = 1000;
            decimal pontuacaoPorQuestao = (decimal)pontuacaoMaxima / (decimal)respostasPontuaveis;
            int quantasAcertadas = 0;

            foreach (var q in todasQuestoes)
            {
                bool possuiAlgumaRespostaErrada = false;
                foreach (var r in respondidas.Where(x => x.CodigoQuestao == q.CodigoQuestao))
                {
                    var acertou = respostasCorretas.Select(x => x.CodigoResposta).Contains(r.CodigoResposta) ? 1 : 0;
                    if (acertou == 0)
                        possuiAlgumaRespostaErrada = true;

                    if (q.UsadaNaPontuacao)
                        quantasAcertadas += acertou;
                }

                q.Acertou = !possuiAlgumaRespostaErrada;
                AtualizarQuestao(q);
            }


            simulado.PontosConquistados = (decimal)(quantasAcertadas * pontuacaoPorQuestao);
            simulado.Percentual = ((decimal)simulado.PontosConquistados) * 100 / pontuacaoMaxima;
            simulado.Aprovado = simulado.Percentual >= 72;
            simulado.Finalizado = simulado.Questoes.All(x => x.Respondidas.Count > 0);
            simulado.TempoDeProvaEmMinutos = (DateTime.Now - simulado.DataDoSimulado).Minutes;
            AtualizarPontuacaoConquistada(simulado);
        }

        private void AtualizarQuestao(Questao q)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("update Questao SET Acertou = @Acertou, UsadaNaPontuacao = @UsadaNaPontuacao WHERE CodigoQuestao = @CodigoQuestao ", q);
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
                int qtdRespostasCorretas = ObterRepostas(codigoPergunta: questao.CodigoPergunta, ativo: true).Count(x => x.Correta);
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

        public List<Questao> ObterQuestao(int codigoSimulado,
            int? numero = null,
            int? codigoPergunta = null,
            int? codigoUsuario = null,
            bool? somenteCorretas = null,
            bool? somenteIncorretas = null,
            bool? usadaNaPontuacao = null)
        {
            List<Questao> questoes = new();
            bool? acertou = null;

            if (somenteCorretas.HasValue && somenteCorretas == true)
                acertou = true;
            else if (somenteIncorretas.HasValue && somenteIncorretas == true)
                acertou = false;

            using (var conn = new SqlConnection(ConnectionString))
                questoes = conn.Query<Questao>("SELECT * FROM Questao WHERE CodigoSimulado = @CodigoSimulado AND Numero = ISNULL(@Numero,Numero) AND Acertou = ISNULL(@Acertou,Acertou) AND UsadaNaPontuacao = ISNULL(@UsadaNaPontuacao,UsadaNaPontuacao) AND CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta);", new { @CodigoSimulado = codigoSimulado, @Numero = numero, @Acertou = acertou, @UsadaNaPontuacao = usadaNaPontuacao, @CodigoPergunta = codigoPergunta }).ToList();

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



        public List<Pergunta> ObterPerguntasPor(int? codigoPergunta = null, int? codigoProva = null, List<int> codigoPerguntaIn = null, int? codigoUsuario = null, bool? ativo = null)
        {
            List<Pergunta> perguntas = new();
            string sqlQuery = $"SELECT * FROM Pergunta WHERE CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta) AND Ativo = ISNULL(@Ativo,Ativo) AND CodigoProva = ISNULL(@CodigoProva,CodigoProva)";
            if (codigoPerguntaIn?.Count > 0)
                sqlQuery += " AND CodigoPergunta IN @CodigoPerguntaIn";
            using (var conn = new SqlConnection(ConnectionString))
            {
                perguntas = conn.Query<Pergunta>(sqlQuery,
                    new
                    {
                        @CodigoPergunta = codigoPergunta,
                        @CodigoProva = codigoProva,
                        @CodigoPerguntaIn = codigoPerguntaIn,
                        @Ativo = ativo
                    }).ToList();
                foreach (var p in perguntas)
                {
                    p.Respostas = ObterRepostas(p.CodigoPergunta, ativo: true);
                    p.Prova = ObterProvas(codigoProva: p.CodigoProva).FirstOrDefault();
                    p.Dominio = ObterDominiosPor(codigoDominio: p.CodigoDominio, codigoProva: p.CodigoProva).FirstOrDefault();
                    p.Anotacao = ObterAnotacao(codigoUsuario: codigoUsuario, codigoPergunta: p.CodigoPergunta);
                }
            }
            return perguntas;
        }

        private List<Pergunta> ObterPerguntasAleatorios(Simulado simulado, bool? ativo = null)
        {
            var rnd = new Random();
            return ObterPerguntasPor(codigoProva: simulado.CodigoProva, ativo: ativo).OrderBy(item => rnd.Next()).Take(simulado.QuantidadeDeQuestoes).ToList();
        }

        private List<Resposta> ObterRepostas(int? codigoPergunta = null, bool? ativo = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Resposta>("SELECT * FROM Resposta WHERE CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta) AND Ativo = ISNULL(@Ativo,Ativo)", new { @CodigoPergunta = codigoPergunta, @Ativo = ativo }).ToList();
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
                Gabarito g = new()
                {
                    NumeroQuestao = q.Numero,
                    Texto = q.Pergunta.Texto,
                    Resposta = string.Join(" | ", q.Pergunta.Respostas.Where(x => x.Correta).Select(x => x.Texto)),
                    Respondida = string.Join(" | ", q.Pergunta.Respostas.Where(y => q.Respondidas.Select(x => x.CodigoResposta).Contains(y.CodigoResposta)).Select(x => x.Texto)),
                    Resultado = q.Acertou ? "Correta" : "Incorreta"
                };
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

        public void InserirAnotacao(Anotacao anotacao)
        {
            using var conn = new SqlConnection(ConnectionString);
            anotacao.CodigoAnotacao = conn.ExecuteScalar<int>("INSERT Anotacao (Texto,CodigoUsuario,CodigoPergunta) VALUES (@Texto,@CodigoUsuario,@CodigoPergunta); SELECT SCOPE_IDENTITY();", anotacao);
        }

        public void EditarAnotacao(int? codigoAnotacao = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Execute("UPDATE Anotacao SET Texto = @Texto WHERE CodigoAnotacao = @CodigoAnotacao", new { @CodigoAnotacao = codigoAnotacao });
        }

        public List<Anotacao> ObterAnotacao(int? codigoUsuario = null, int? codigoPergunta = null)
        {
            using var conn = new SqlConnection(ConnectionString);
            return conn.Query<Anotacao>("SELECT * FROM Anotacao WHERE CodigoUsuario = ISNULL(@CodigoUsuario,CodigoUsuario) AND CodigoPergunta = ISNULL(@CodigoPergunta,CodigoPergunta);", new { @CodigoUsuario = codigoUsuario, @CodigoPergunta = codigoPergunta }).ToList();
        }



    }
}
