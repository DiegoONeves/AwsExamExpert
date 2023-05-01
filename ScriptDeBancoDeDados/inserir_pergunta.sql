declare @CodigoPerguntaInserida int = 0;

insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(
1,
3,--Código Prova
2, -- Domínio
1,--Multipla escolha
'Uma equipe de engenharia deseja examinar a viabilidade do recurso de dados do usuário do Amazon EC2 para um projeto futuro.

Quais das seguintes opções são verdadeiras sobre a configuração de dados do usuário do EC2? (Selecione dois)');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Configure o aplicativo para implantar diretamente a nova versão do Lambda. Se a implantação der errado, redefina o aplicativo de volta para a versão atual usando o número da versão no ARN'),
(1,@CodigoPerguntaInserida,0,'Configure o aplicativo para ter vários alias da função Lambda. Implante a nova versão do código. Configure um novo alias que aponte para o alias atual da função Lambda para lidar com 10% do tráfego. Se a implantação der errado, redefina o novo alias para apontar todo o tráfego para o alias de trabalho mais recente da função Lambda'),
(1,@CodigoPerguntaInserida,1,'Configure o aplicativo para usar um alias que aponte para a versão atual. Implante a nova versão do código e configure o alias para enviar 10% dos usuários para esta nova versão. Se a implantação der errado, redefina o alias para apontar todo o tráfego para a versão atual'),
(1,@CodigoPerguntaInserida,0,'Configure o aplicativo para usar um alias que aponte para a versão atual. Implante a nova versão do código e configure o alias para enviar todos os usuários para esta nova versão. Se a implantação der errado, redefina o alias para apontar para a versão atual')


--select * from Dominio where CodigoProva = 2
select * from Pergunta where Texto like '%DAX%'
select * from Resposta where CodigoPergunta = 96
update Pergunta
set Ativo = 0
where CodigoPergunta IN(20)

update Pergunta
set MultiplaEscolha = 1,
CodigoDominio = 4
where CodigoPergunta = 81

delete from Resposta where CodigoResposta = 214

update Resposta
set Correta = 0
where CodigoResposta in(400)

update Resposta
set Ativo = 0
where CodigoResposta in(214)

update Pergunta
set Texto = Replace(Texto,'Na frente das instâncias está um balanceador de carga','Na frente das instâncias está um load balancer')


update Resposta
set Texto = Replace(Texto,'implantação azul/verde','implantação Blue/Green')

SELECT MAX(CodigoPergunta)
FROM Pergunta 
WHERE CodigoPergunta IN












