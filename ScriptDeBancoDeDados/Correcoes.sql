select * from Pergunta where Texto like 'Voc� est� planejando implantar um aplicativo m�vel%'
select * from Resposta where CodigoPergunta = 64
select * from Dominio where CodigoDominio = 4


update Resposta 
set Texto = Replace(Texto,'uma pol�tica e uma fun��o','uma pol�tica e uma role')

select * from Pergunta where CodigoDominio = 1
