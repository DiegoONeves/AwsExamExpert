USE SimuladoCertificacao
GO 
declare @CodigoPerguntaInserida int = 0;
insert into Pergunta (Ativo,CodigoProva,MultiplaEscolha,Texto) values(
1,
3,--Código Prova
0,--Multipla escolha
'Uma empresa contratou você como arquiteto de soluções certificado pela AWS – associado para ajudar no redesenho de um processador de dados em tempo real. A empresa deseja construir aplicativos personalizados que processem e analisem os dados de streaming para suas necessidades especializadas.

Qual solução você recomendaria para resolver esse caso de uso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Use o Amazon Kinesis Data Streams para processar os fluxos de dados, bem como desacoplar os produtores e consumidores para o processador de dados em tempo real')
,(1,@CodigoPerguntaInserida,0,'Use o Amazon Kinesis Data Firehose para processar os fluxos de dados, bem como desacoplar os produtores e consumidores para o processador de dados em tempo real')
,(1,@CodigoPerguntaInserida,0,'Use o Amazon Simple Notification Service (Amazon SNS) para processar os fluxos de dados, bem como desacoplar os produtores e consumidores para o processador de dados em tempo real')
,(1,@CodigoPerguntaInserida,0,'Use o Amazon Simple Queue Service (Amazon SQS) para processar os fluxos de dados, bem como desacoplar os produtores e consumidores para o processador de dados em tempo real')
--,(1,@CodigoPerguntaInserida,0,'')
--,(1,@CodigoPerguntaInserida,0,'')
SELECT * FROM Pergunta WHERE CodigoProva = 3
