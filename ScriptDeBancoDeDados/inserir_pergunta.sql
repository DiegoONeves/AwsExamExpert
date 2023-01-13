
declare @CodigoPerguntaInserida int = 0;

insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,2,3,0,
'Você tem uma VPC com uma sub-rede que hospeda uma instância do AWS RDS. Seu gerente pediu para você começar a monitorar todas as modificações feitas em seus dados pelos aplicativos. Você decide usar uma função do Lambda para fazer isso. No entanto, você precisa permitir o acesso da função ao VPC. Como você vai fazer isso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Crie uma função para permitir o acesso entre Lambda e RDS.'),
(1,@CodigoPerguntaInserida,0,'Crie uma política e uma função para permitir o acesso entre o Lambda e o RDS.'),
(1,@CodigoPerguntaInserida,0,'Crie uma função para permitir o acesso entre o Lambda e a sub-rede.'),
(1,@CodigoPerguntaInserida,1,'Crie uma função para permitir o acesso entre Lambda e VPC.')



select * from Dominio where CodigoProva = 2
select * from Pergunta
