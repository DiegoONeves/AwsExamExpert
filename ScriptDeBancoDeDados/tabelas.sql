USE master;
go

DROP Database SimuladoCertificacao;

go

CREATE DATABASE SimuladoCertificacao;

GO

USE SimuladoCertificacao;

GO

create table Usuario
(
	CodigoUsuario int identity(1,1) primary key,
	Nome varchar(200) not null,
	WhatsApp varchar(20) not null,
	Administrador bit not null default 0,
	Liberado bit not null,
	DataDeCadastro datetime not null,
	DataDeVencimento datetime null
)

create table Prova
(
	CodigoProva int identity(1,1) primary key,
	Descricao varchar(200) not null,
	Ativo bit not null
)

create table Dominio
(
	CodigoDominio int identity(1,1) primary key,
	Descricao varchar(255) not null,
	CodigoProva int foreign key references Prova(CodigoProva) not null
)

CREATE TABLE Pergunta
(
	CodigoPergunta int identity(1,1) primary key,
	CodigoProva int foreign key references Prova(CodigoProva) not null,
	CodigoDominio int foreign key references Dominio(CodigoDominio) not null,
	MultiplaEscolha bit not null,
	Texto varchar(max) not null,
	Ativo bit not null
);

CREATE TABLE Resposta
(
	CodigoResposta int identity(1,1) primary key,
	CodigoPergunta int not null foreign key references Pergunta(CodigoPergunta),
	Texto varchar(max) not null,
	Correta bit not null,
	Ativo bit not null
);

Create Table Simulado
(
	CodigoSimulado int identity(1,1) primary key,
	CodigoUsuario int foreign key references Usuario(CodigoUsuario) not null,
	CodigoProva int foreign key references Prova(CodigoProva) not null,
	DataDoSimulado datetime not null,
	QuantidadeDeQuestoes int not null,
	PontosDisputados int not null,
	PontosConquistados decimal not null,
	Percentual decimal not null,
	Aprovado bit not null,
	TempoDeProvaEmMinutos int null,
	Finalizado bit not null
);
           

create table Questao
(
	CodigoQuestao int identity (1,1) primary key,
	CodigoSimulado int not null foreign key references Simulado(CodigoSimulado),
	Numero int not null,
	CodigoPergunta int not null foreign key references Pergunta(CodigoPergunta)
);

create table Respondida
(
	CodigoQuestao int foreign key references Questao(CodigoQuestao) not null,
	CodigoResposta int foreign key references Resposta(CodigoResposta) not null
)

create table OrdemResposta
(
	CodigoSimulado int foreign key references Simulado(CodigoSimulado) not null,
	CodigoPergunta int foreign key references Pergunta(CodigoPergunta) not null,
	CodigoQuestao int foreign key references Questao(CodigoQuestao) not null,
	CodigoResposta int foreign key references Resposta(CodigoResposta) not null,
	Ordem varchar(20) not null

)

create table LogDeErro
(
	CodigoLogDeErro int identity(1,1) primary key,
	Erro varchar(max) not null,
	Acao varchar(200) not null,
	CodigoUsuario int null,
	DataHoraDoErro datetime not null
)


