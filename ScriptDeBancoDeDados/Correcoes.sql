select * from Pergunta where Texto like 'Você está planejando implantar um aplicativo móvel%'
select * from Resposta where CodigoPergunta = 64
select * from Dominio where CodigoDominio = 4


update Resposta 
set Texto = Replace(Texto,'uma política e uma função','uma política e uma role')

select * from Pergunta where CodigoDominio = 1
