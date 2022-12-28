declare @CodigoPerguntaInserida int = 0;

insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,2,3,0,
'Você é um desenvolvedor de API para uma grande empresa de manufatura. Você desenvolveu um recurso de API que adiciona novos produtos ao inventário do distribuidor usando uma solicitação POST HTTP. Ele inclui um cabeçalho Origin e aceita application/x-www-form-encoded como tipo de conteúdo de solicitação. Qual cabeçalho de resposta permitirá o acesso a este recurso de outra origem?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Access-Control-Allow-Origin.'),
(1,@CodigoPerguntaInserida,0,'Access-Control-Request-Method'),
(1,@CodigoPerguntaInserida,0,'Access-Control-Request-Headers'),
(1,@CodigoPerguntaInserida,0,'Nenhuma das anteriores'),
(1,@CodigoPerguntaInserida,0,'A e B')



--select * from Dominio where CodigoProva = 2
select * from Pergunta
