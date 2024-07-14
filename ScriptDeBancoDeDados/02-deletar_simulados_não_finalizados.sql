USE SimuladoCertificacao
GO
delete from Respondida where CodigoQuestao in (select CodigoQuestao from Questao where CodigoSimulado in (select CodigoSimulado from Simulado where Finalizado = 0))
delete from OrdemResposta where CodigoQuestao in (select CodigoQuestao from Questao where CodigoSimulado in (select CodigoSimulado from Simulado where Finalizado = 0)) 
delete from Questao where CodigoSimulado in (select CodigoSimulado from Simulado where Finalizado = 0)
delete from Simulado where Finalizado = 0