use SimuladoCertificacao;
GO

--inserindo administrador
insert into Usuario (Nome,WhatsApp,Administrador,Liberado,DataDeCadastro) values('Administrador', '11949009387', 1,1,getdate())

go

declare @CodigoPerguntaInserida int = 0;
declare @CodigoProvaInserida int = 0;

--inserindo a prova 1
insert into Prova (Descricao,Ativo) values ('AWS Certified Cloud Practitioner',1)
select @CodigoProvaInserida = MAX(CodigoProva) from Prova

----pergunta 1
--insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,1, 0,'Qual serviço da AWS fornece recomendações de otimização de segurança de infraestrutura?');
--select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
--insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
--(1,@CodigoPerguntaInserida,0,'AWS Application Programming Interface(API)'),
--(1,@CodigoPerguntaInserida,0,'Instâncias Reservadas'),
--(1,@CodigoPerguntaInserida,1,'AWS Trusted Advisor'),
--(1,@CodigoPerguntaInserida,0,'AWS Elastic Compute Cloud (AWS EC2) SpotFleet')

----pergunta 1
--insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,1, 0,'Um serviço de compartilhamento de arquivos usa o AWS S3 para armazenar arquivos carregados pelos usuários. Os arquivos são acessados ??com frequência aleatória. Os populares são baixados todos os dias, enquanto outros não com tanta frequência e alguns raramente. Qual é a classe de armazenamento de objetos do AWS S3 mais econômica para implementar?');
--select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
--insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
--(1,@CodigoPerguntaInserida,0,'AWS S3 Standard'),
--(1,@CodigoPerguntaInserida,0,'AWS S3 Glacier'),
--(1,@CodigoPerguntaInserida,0,'AWS S3 One Zone-Infrequently Accessed'),
--(1,@CodigoPerguntaInserida,1,'AWS S3 Intelligent-Tiering')

--inserindo a prova 1
insert into Prova (Descricao,Ativo) values ('AWS Certified Developer - Associate',1)
select @CodigoProvaInserida = MAX(CodigoProva) from Prova

--inserindo domínios da prova 1
insert into Dominio (CodigoProva,Descricao) values (@CodigoProvaInserida,'Domínio 1: Implantação'),
(@CodigoProvaInserida,'Domínio 1: Implantação'),
(@CodigoProvaInserida,'Domínio 2: Segurança'),
(@CodigoProvaInserida,'Domínio 3: Desenvolvimento com os produtos da AWS'),
(@CodigoProvaInserida,'Domínio 4: Refatoração'),
(@CodigoProvaInserida,'Domínio 5: Monitoramento e resolução de problemas')

--pergunta 1
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,1,'Uma organização implantou seus sites estáticos no AWS S3. Agora, o desenvolvedor tem um requisito para fornecer conteúdo dinâmico usando uma solução sem servidor. Qual combinação de serviços deve ser usada para implementar um aplicativo sem servidor para o conteúdo dinâmico? (Selecione duas)');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'AWS API Gateway'),
(1,@CodigoPerguntaInserida,0,'AWS EC2'),
(1,@CodigoPerguntaInserida,0,'AWS ECS'),
(1,@CodigoPerguntaInserida,1,'AWS Lambda'),
(1,@CodigoPerguntaInserida,0,'AWS Kinesis');

--pergunta 2
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Um desenvolvedor foi solicitado a criar um ambiente AWS Elastic Beanstalk para um aplicativo web de produção que precisa lidar com milhares de solicitações. Atualmente, o ambiente dev está executando uma instância t1.micro. Qual é a melhor maneira para o desenvolvedor provisionar um novo ambiente de produção com uma instância m4.large em vez de uma t1.micro?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Use o CloudFormation para migrar o tipo de instância do AWS EC2 de t1.micro para m4.large.'),
(1,@CodigoPerguntaInserida,1,'Crie um novo arquivo de configuração com o tipo de instância como m4.large e faça referência a esse arquivo ao provisionar o novo ambiente.'),
(1,@CodigoPerguntaInserida,0,'Provisione uma instância m4.large diretamente no ambiente de desenvolvimento e implante no novo ambiente de produção.'),
(1,@CodigoPerguntaInserida,0,'Altere o valor do tipo de instância no arquivo de configurações para m4.large usando o comando da CLI update autoscaling group.')


--pergunta 3
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Você está usando modelos do AWS SAM para implantar um aplicativo serverless. Qual dos seguintes recursos incorporará aplicativos aninhados de buckets do AWS S3?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'AWS::Serverless::API'),
(1,@CodigoPerguntaInserida,1,'AWS::Serverless::Application'),
(1,@CodigoPerguntaInserida,0,'AWS::Serverless::LayerVersion'),
(1,@CodigoPerguntaInserida,0,'AWS::Serverless::Function')


--pergunta 4
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,4,0,'Um aplicativo hospedado na AWS foi configurado para usar uma tabela do DynamoDB. Vários itens são gravados na tabela do DynamoDB. Como parte de uma estratégia de arquivamento, esses itens estarão acessíveis em um determinado período de tempo, após o qual poderão ser arquivados e excluídos. Qual das opções a seguir é uma maneira ideal de gerenciar a exclusão dos itens obsoletos?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Execute uma varredura na tabela para os itens obsoletos e emita a operação Delete.'),
(1,@CodigoPerguntaInserida,0,'Crie uma coluna adicional para armazenar a data. Execute uma consulta para os objetos obsoletos e execute a operação Delete.'),
(1,@CodigoPerguntaInserida,0,'Habilite o controle de versão para os itens no DynamoDB e exclua a última versão acessada.'),
(1,@CodigoPerguntaInserida,1,'Habilite o TTL para os itens no DynamoDB.')


--pergunta 5
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Você criou os seguintes estágios no CodePipeline: Source => Build => Staging. O que acontece se houver uma falha detectada no estágio "Build"?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Uma reversão acontecerá no estágio "Fonte".'),
(1,@CodigoPerguntaInserida,0,'A etapa "Build" será tentada novamente.'),
(1,@CodigoPerguntaInserida,0,'A etapa "Build" será ignorada e a etapa "Staging" será iniciada.'),
(1,@CodigoPerguntaInserida,1,'Todo o processo será interrompido.')


--pergunta 6
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,5,0,'Sua equipe acabou de desenvolver uma nova versão de um aplicativo existente. Este é um aplicativo baseado na web hospedado na AWS. Atualmente, o Route 53 está sendo usado para apontar o nome DNS da empresa para o site. Sua Gerência o instruiu a entregar o novo aplicativo a uma parte dos usuários para teste. Como você pode conseguir isso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Transfira o aplicativo para o Elastic beanstalk e use o recurso Swap URL.'),
(1,@CodigoPerguntaInserida,1,'Use as políticas de roteamento ponderadas do Route 53.'),
(1,@CodigoPerguntaInserida,0,'Transfira o aplicativo para o Opswork criando uma nova pilha.'),
(1,@CodigoPerguntaInserida,0,'Use as políticas de roteamento de failover do Route 53.')


--pergunta 7
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,4,0,'Você está desenvolvendo um aplicativo que será hospedado em uma instância do EC2. Isso fará parte de um Autoscaling group. O aplicativo precisa obter o IP privado da instância para enviá-lo para um aplicativo baseado em controlador. Qual dos seguintes pode ser feito para conseguir isso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Consultar os metadados da instância.'),
(1,@CodigoPerguntaInserida,0,'Consultar os dados do usuário da instância.'),
(1,@CodigoPerguntaInserida,0,'Faça com que um administrador obtenha o endereço IP do console.'),
(1,@CodigoPerguntaInserida,0,'Faça o aplicativo executar IFConfig.')


--pergunta 8
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Seu aplicativo de análise de log atual leva mais de quatro horas para gerar um relatório dos 10 principais usuários do seu aplicativo da web. Você foi solicitado a implementar um sistema que possa relatar essas informações em tempo real, garantir que o relatório esteja sempre atualizado e lidar com aumentos no número de solicitações para seu aplicativo da web. Escolha a opção que seja econômica e possa atender aos requisitos.');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Publique seus dados no CloudWatch Logs e configure seu aplicativo no Autoscale para lidar com a carga sob demanda.'),
(1,@CodigoPerguntaInserida,0,'Publique seus dados de log em um bucket do AWS S3. Use o AWS CloudFormation para criar um grupo de Auto Scaling para dimensionar seu aplicativo de pós-processamento que está configurado para baixar seus arquivos de log armazenados em um AWS S3.'),
(1,@CodigoPerguntaInserida,1,'Poste seus dados de log em um fluxo de dados do AWS Kinesis e inscreva seu aplicativo de processamento de log para que seja configurado para processar seus dados de log.'),
(1,@CodigoPerguntaInserida,0,'Crie um cluster MySQL do AWS RDS multi-AZ, publique os dados de log no MySQL e execute um trabalho de redução de mapa para recuperar as informações necessárias sobre contagens de usuários.')


--pergunta 9
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Um aplicativo tem usado o AWS DynamoDB para seu armazenamento de dados de back-end. O tamanho da tabela agora aumentou para 20 GB e as verificações na tabela estão causando erros de limitação. Qual dos seguintes deve ser implementado agora para evitar tais erros?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Aumentar o tamanho da paginação.'),
(1,@CodigoPerguntaInserida,1,'Reduzir o tamanho da paginação.'),
(1,@CodigoPerguntaInserida,0,'Scans paralelos.'),
(1,@CodigoPerguntaInserida,0,'Scans sequenciais.')


--pergunta 10
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,2,1,'Você desenvolveu um conjunto de scripts usando o AWS Lambda. Esses scripts precisam acessar instâncias do EC2 em uma VPC. Qual das opções a seguir precisa ser feita para garantir que a função AWS Lambda possa acessar os recursos na VPC? (Selecione DOIS)');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Certifique-se de que os IDs de sub-rede estejam configurados na função do Lambda.'),
(1,@CodigoPerguntaInserida,0,'Certifique-se de que os IDs NACL estejam configurados na função Lambda.'),
(1,@CodigoPerguntaInserida,1,'Certifique-se de que os IDs do grupo de segurança estejam configurados na função do Lambda.'),
(1,@CodigoPerguntaInserida,0,'Certifique-se de que os IDs de log de fluxo da VPC estejam configurados na função do Lambda.')



--pergunta 11
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Você está desenvolvendo um aplicativo que fará uso do AWS Kinesis. Devido à alta taxa de transferência, você decide ter vários shards para os fluxos. Qual das opções a seguir é VERDADEIRA quando se trata de processar dados em vários shards?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Você não pode garantir a ordem dos dados em vários shards. Só é possível dentro de um shard.'),
(1,@CodigoPerguntaInserida,0,'A ordem dos dados é possível em todos os shards em um fluxo.'),
(1,@CodigoPerguntaInserida,0,'A ordem dos dados não é possível em streams do Kinesis.'),
(1,@CodigoPerguntaInserida,0,'Você precisa usar o Kinesis firehose para garantir a ordem dos dados.')


--pergunta 12
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Sua empresa solicitou a manutenção de um aplicativo usando o Elastic Beanstalk. Às vezes, você normalmente atinge o limite de versão do aplicativo ao implantar novas versões do aplicativo. Qual das opções a seguir é a maneira mais eficaz de gerenciar esse problema?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Crie vários ambientes e implante as diferentes versões em diferentes ambientes.'),
(1,@CodigoPerguntaInserida,1,'Crie uma política de ciclo de vida da versão do aplicativo.'),
(1,@CodigoPerguntaInserida,0,'Crie vários aplicativos e implante as diferentes versões em diferentes aplicativos.'),
(1,@CodigoPerguntaInserida,0,'Exclua as versões do aplicativo manualmente.')

--pergunta 13
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Como desenvolvedor, você habilitou o log do servidor em um bucket do S3. Você tem uma página da Web estática simples com páginas CSS carregadas no bucket que tem 1 MB de tamanho total. Após uma duração de 2 semanas, você volta e vê que o tamanho do bucket aumentou para 50 MB. Qual dos seguintes poderia ser uma razão para isso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Você também ativou o CRR no bucket. É por isso que o espaço está sendo consumido.'),
(1,@CodigoPerguntaInserida,0,'Você também ativou a criptografia no bucket. É por isso que o espaço está sendo consumido.'),
(1,@CodigoPerguntaInserida,1,'Os logs de acesso ao servidor são configurados para serem entregues no mesmo bucket que o bucket de origem.'),
(1,@CodigoPerguntaInserida,0,'O monitoramento foi ativado para o bucket.')


--pergunta 14----
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Uma empresa possui um modelo Cloudformation que é usado para criar uma enorme lista de recursos. Ele cria uma VPC, sub-redes, instâncias do EC2, grupos de escalonamento automático, balanceadores de carga etc. Qual dos seguintes deve ser considerado ao projetar esses modelos de Cloudformation?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Certifique-se de criar uma pilha inteira do modelo.'),
(1,@CodigoPerguntaInserida,1,'Procure dividir os modelos em modelos menores e gerenciáveis.'),
(1,@CodigoPerguntaInserida,0,'Empacote os modelos e use o comando cloudformation deploy.'),
(1,@CodigoPerguntaInserida,0,'Empacote os modelos e use o comando cloudformation package.')

--pergunta 15
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Um aplicativo tem um banco de dados em uma instância do AWS RDS. Quando o tráfego é alto, o tempo de resposta do aplicativo aumenta, pois há muitas consultas de leitura no banco de dados RDS. Qual das opções a seguir pode ser usada para diminuir o tempo de resposta do aplicativo?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Coloque uma distribuição do CloudFront na frente do banco de dados.'),
(1,@CodigoPerguntaInserida,1,'Habilite Réplicas de Leitura para o banco de dados.'),
(1,@CodigoPerguntaInserida,0,'Altere o banco de dados de RDS para DynamoDB.'),
(1,@CodigoPerguntaInserida,0,'Habilite o Multi-AZ para o banco de dados.')


--pergunta 16
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,1,'Você implantou um aplicativo em uma instância do EC2. Esse aplicativo faz chamadas para um serviço do DynamoDB. Existem vários problemas de desempenho presentes no aplicativo. Você decide usar o serviço XRay para depurar os problemas de desempenho. Você não consegue ver as trilhas no serviço XRay. Qual dos seguintes poderia ser o problema subjacente? Escolha 2 respostas entre as opções dadas abaixo.');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'O daemon do X-Ray não está instalado na instância do EC2.'),
(1,@CodigoPerguntaInserida,0,'A AMI correta não é escolhida para a instância do EC2.'),
(1,@CodigoPerguntaInserida,1,'Certifique-se de que a função do IAM anexada à instância tenha permissão para fazer upload de dados no X-Ray.'),
(1,@CodigoPerguntaInserida,0,'Certifique-se de que a função do IAM anexada à instância tenha permissão para fazer upload de dados no Cloudwatch.')


--pergunta 17
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Você está planejando implantar um aplicativo para a função de worker no Elastic Beanstalk. Além disso, este aplicativo de trabalho executará as tarefas periódicas. Qual das opções a seguir é obrigatória como parte da implantação?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Um arquivo appspec.yaml'),
(1,@CodigoPerguntaInserida,1,'Um arquivo cron.yaml'),
(1,@CodigoPerguntaInserida,0,'Um arquivo cron.config'),
(1,@CodigoPerguntaInserida,0,'Um arquivo appspec.json')


--pergunta 18
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,2,0,'Você está desenvolvendo um aplicativo que será hospedado no AWS Lambda. A função fará chamadas para um banco de dados. Um requisito é que todas as cadeias de conexão do banco de dados sejam mantidas seguras. Qual das opções a seguir é a maneira MAIS segura de implementar isso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Coloque os valores das strings de conexão em um modelo do CloudFormation.'),
(1,@CodigoPerguntaInserida,0,'Coloque a string de conexão do banco de dados no arquivo app.json e armazene-a em um repositório Git.'),
(1,@CodigoPerguntaInserida,1,'O Lambda precisa fazer referência ao AWS Systems Manager Parameter Store para a string de conexão do banco de dados criptografada.'),
(1,@CodigoPerguntaInserida,0,'Coloque a string de conexão do banco de dados na própria função do AWS Lambda, pois todas as funções do Lambda são criptografadas em repouso.')


--pergunta 19
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,4,0,'Atualmente, uma empresa possui um aplicativo que funciona com o DynamoDB. O aplicativo é um aplicativo de alta geração de receita para a empresa. Seu tempo de resposta atual para suas cargas de trabalho de leitura é da ordem de milissegundos. Mas para aumentar os acessos às suas páginas, eles querem reduzir o tempo de resposta para microssegundos. Qual das opções a seguir você sugeriria para ser usado com mais preferência com o DynamoDB para atender a esse requisito?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Considere implantar um ElastiCache na frente do DynamoDB.'),
(1,@CodigoPerguntaInserida,0,'Considere usar tabelas globais do DynamoDB.'),
(1,@CodigoPerguntaInserida,1,'Considere usar o acelerador DynamoDB.'),
(1,@CodigoPerguntaInserida,0,'Considere usar uma taxa de transferência mais alta para as tabelas.')

--pergunta 20
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Qual das seguintes afirmações é verdadeira em relação a solicitações de leitura fortemente consistentes de um aplicativo para um DynamoDB com um cluster DAX?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Todas as solicitações são encaminhadas ao DynamoDB e os resultados são armazenados em cache.'),
(1,@CodigoPerguntaInserida,0,'Todas as solicitações são encaminhadas ao DynamoDB e os resultados são armazenados no Cache de itens antes de passar para o aplicativo.'),
(1,@CodigoPerguntaInserida,0,'Todas as solicitações são encaminhadas ao DynamoDB e os resultados são armazenados no Query Cache antes de passar para o aplicativo.'),
(1,@CodigoPerguntaInserida,1,'Todas as solicitações são encaminhadas ao DynamoDB e os resultados não são armazenados em cache.')


--pergunta 21
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Seu líder de equipe terminou de criar um projeto de compilação no console usando o AWS CodeBuild. Você tem acesso para executar a compilação, mas não tem acesso ao projeto. Você deseja especificar um local de origem diferente para a compilação. Como você pode conseguir isso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Emita o comando update project e especifique o novo local do build.'),
(1,@CodigoPerguntaInserida,0,'Especifique o novo local da compilação em um novo arquivo buildspec.yml e emita o comando update-project.'),
(1,@CodigoPerguntaInserida,1,'Especifique o novo local da compilação em um novo arquivo buildspec.yml e use o comando start-build.'),
(1,@CodigoPerguntaInserida,0,'Especifique o novo local da compilação em um novo arquivo buildspec.yml e use o comando update-build.')

--pergunta 22
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,4,0,'Você acabou de criar uma função do AWS Lambda. Você está executando a função, mas a saída da função não é a esperada. Você precisa verificar e ver qual é o problema. Qual das opções a seguir pode ajudar o desenvolvedor a depurar o problema com a função do Lambda?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Check Cloudwatch logs'),
(1,@CodigoPerguntaInserida,0,'Check VPC Flow Logs'),
(1,@CodigoPerguntaInserida,0,'Check AWS Trusted Advisor'),
(1,@CodigoPerguntaInserida,0,'Check AWS Inspector')

--pergunta 23
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Você foi contratado como desenvolvedor para trabalhar em um aplicativo. Este aplicativo é hospedado em uma instância do EC2 e interage com uma fila SQS. Foi notado que quando o aplicativo está puxando mensagens, muitas respostas vazias estão sendo retornadas. Que alteração você pode fazer para garantir que o aplicativo use a fila SQS de forma eficaz?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Use long polling'),
(1,@CodigoPerguntaInserida,0,'Defina um tempo limite de visibilidade personalizado'),
(1,@CodigoPerguntaInserida,0,'Use short polling'),
(1,@CodigoPerguntaInserida,0,'Implemente a retirada exponencial')

--pergunta 24
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,0,'Sua equipe atualmente desenvolveu um aplicativo usando contêineres do Docker. Como líder de desenvolvimento, agora você precisa hospedar esse aplicativo na AWS. Você também precisa garantir que o serviço da AWS tenha serviços de orquestração integrados. Qual dos seguintes pode ser usado para esta finalidade?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Considere construir um cluster Kubernetes em instâncias do EC2.'),
(1,@CodigoPerguntaInserida,0,'Crie vários aplicativos e implante as diferentes versões em diferentes aplicativos.'),
(1,@CodigoPerguntaInserida,1,'Considere usar o Elastic Container Service.'),
(1,@CodigoPerguntaInserida,0,'Considere usar o serviço Simple Storage para armazenar seus contêineres docker.')

--pergunta 25
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,5,0,'Você está desenvolvendo um aplicativo que está trabalhando com uma tabela do DynamoDB. Durante a fase de desenvolvimento, você deseja saber quanto da capacidade Consumida está sendo usada para as consultas que estão sendo disparadas. Como isso pode ser alcançado?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'As consultas por padrão enviadas pelo programa retornarão a capacidade consumida como parte do resultado.'),
(1,@CodigoPerguntaInserida,0,'Certifique-se de definir ReturnConsumedCapacity na solicitação de consulta como TRUE.'),
(1,@CodigoPerguntaInserida,1,'Certifique-se de definir ReturnConsumedCapacity na solicitação de consulta para TOTAL.'),
(1,@CodigoPerguntaInserida,0,'Use a operação Scan em vez da operação de consulta.')

--pergunta 26
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,1,'Sua equipe de desenvolvimento está planejando trabalhar com o AWS Step Functions. Qual das opções a seguir é uma prática recomendada ao trabalhar com workers de atividades e tarefas no Step Functions? Escolha 2 respostas entre as opções dadas abaixo.');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'Certifique-se de especificar um tempo limite nas definições da máquina de estado.'),
(1,@CodigoPerguntaInserida,0,'Podemos usar apenas 1 transição por estado.'),
(1,@CodigoPerguntaInserida,1,'Se você estiver passando cargas maiores entre estados, considere usar o Simple Storage Service.'),
(1,@CodigoPerguntaInserida,0,'Se você estiver transmitindo cargas úteis maiores entre estados, considere o uso de volumes do EBS.')

--pergunta 27
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,5,0,'Sua equipe começou a configurar o CodeBuild para executar compilações na AWS. O código-fonte é armazenado em um bucket. Quando a compilação é executada, você está recebendo o erro abaixo: Erro: "O bucket que você está tentando acessar deve ser endereçado usando o endpoint especificado..." Ao executar uma compilação. Qual dos seguintes poderia ser a causa do erro?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,1,'O bucket não está na mesma região que o projeto Code Build.'),
(1,@CodigoPerguntaInserida,0,'Idealmente, o código deve ser armazenado em Volumes EBS.'),
(1,@CodigoPerguntaInserida,0,'O controle de versão está habilitado para o bucket.'),
(1,@CodigoPerguntaInserida,0,'A MFA está habilitada no bucket.')

--pergunta 28
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,1,1,'Sua equipe está atualmente gerenciando um conjunto de aplicativos para uma empresa na AWS. Agora há um requisito para realizar implantações Blue Green para o futuro conjunto de aplicativos. Qual dos seguintes pode ajudá-lo a conseguir isso? Escolha 2 respostas entre as opções dadas abaixo.');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Use Route53 com a política de roteamento de failover.'),
(1,@CodigoPerguntaInserida,1,'Use Route53 com a política de roteamento ponderada.'),
(1,@CodigoPerguntaInserida,1,'Certifique-se de que o aplicativo seja colocado atrás de um ELB.'),
(1,@CodigoPerguntaInserida,0,'Certifique-se de que o aplicativo seja colocado em uma única AZ.')


--pergunta 29
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Uma empresa contratou você para seu projeto de desenvolvimento em andamento. O projeto envolve o streaming de dados para streams do AWS Kinesis de várias fontes de log. Você precisa analisar dados em tempo real usando SQL padrão. Qual dos seguintes pode ser usado para esta finalidade?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'AWS Kinesis Firehose'),
(1,@CodigoPerguntaInserida,1,'AWS Kinesis Data Analytics'),
(1,@CodigoPerguntaInserida,0,'AWS Athena'),
(1,@CodigoPerguntaInserida,0,'AWS EMR')


--pergunta 30
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Você é desenvolvedor de uma empresa. Você precisa desenvolver um aplicativo que transfira os logs de várias instâncias do EC2 para um bucket do S3. Qual dos seguintes você usaria para esse fim?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'AWS Database Migration Service'),
(1,@CodigoPerguntaInserida,0,'AWS Athena'),
(1,@CodigoPerguntaInserida,1,'AWS Data Pipeline'),
(1,@CodigoPerguntaInserida,0,'AWS EMR')

--pergunta 31
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,5,0,'Qual serviço da AWS fornece recomendações de otimização de segurança de infraestrutura?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'AWS Application Programming Interface(API)'),
(1,@CodigoPerguntaInserida,0,'Instâncias Reservadas'),
(1,@CodigoPerguntaInserida,1,'AWS Trusted Advisor'),
(1,@CodigoPerguntaInserida,0,'AWS Elastic Compute Cloud (AWS EC2) SpotFleet')

--pergunta 32
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Um serviço de compartilhamento de arquivos usa o AWS S3 para armazenar arquivos carregados pelos usuários. Os arquivos são acessados ??com frequência aleatória. Os populares são baixados todos os dias, enquanto outros não com tanta frequência e alguns raramente. Qual é a classe de armazenamento de objetos do AWS S3 mais econômica para implementar?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'AWS S3 Standard'),
(1,@CodigoPerguntaInserida,0,'AWS S3 Glacier'),
(1,@CodigoPerguntaInserida,0,'AWS S3 One Zone-Infrequently Accessed'),
(1,@CodigoPerguntaInserida,1,'AWS S3 Intelligent-Tiering')

--pergunta 33
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,0,'Um site de jogos oferece aos usuários a capacidade de trocar itens de jogos entre si na plataforma. A plataforma exige que os registros de ambos os usuários sejam atualizados e persistidos em uma transação. Se alguma atualização falhar, a transação deverá ser revertida.
Qual solução da AWS pode fornecer o recurso transacional necessário para esse recurso?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Amazon DynamoDB com operações feitas com o parâmetro Consistent Read definido como true'),
(1,@CodigoPerguntaInserida,0,'Amazon ElastiCache for Memcached com operações feitas em um bloco de transação'),
(1,@CodigoPerguntaInserida,0,'Amazon DynamoDB com leituras e gravações feitas usando operações Transact'),
(1,@CodigoPerguntaInserida,1,'Amazon Aurora MySQL com operações feitas em um bloco de transação'),
(1,@CodigoPerguntaInserida,0,'Amazon Athena com operações feitas dentro de um bloco de transação')


--pergunta 34
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,5,0,'Um desenvolvedor criou um aplicativo Java que faz solicitações HTTP diretamente aos serviços da AWS. O log do aplicativo mostra códigos de resposta HTTP 5xx que ocorrem em intervalos irregulares. Os erros estão afetando os usuários.
Como o desenvolvedor deve atualizar o aplicativo para melhorar a resiliência do aplicativo?');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Revise o conteúdo da solicitação no código do aplicativo.'),
(1,@CodigoPerguntaInserida,1,'Use o AWS SDK for Java para interagir com APIs da AWS.'),
(1,@CodigoPerguntaInserida,0,'Escale horizontalmente o aplicativo para que mais instâncias do aplicativo estejam em execução.'),
(1,@CodigoPerguntaInserida,0,'Adicione log adicional ao código do aplicativo.'),
(1,@CodigoPerguntaInserida,0,'Amazon Athena com operações feitas dentro de um bloco de transação')



--pergunta 35
insert into Pergunta (Ativo,CodigoProva,CodigoDominio,MultiplaEscolha,Texto) values(1,@CodigoProvaInserida,3,1,'Um desenvolvedor projetou um aplicativo em uma instância do Amazon EC2. O aplicativo faz solicitações de API para objetos em um bucket do Amazon S3.
Qual combinação de etapas garantirá que o aplicativo faça as solicitações de API da maneira MAIS segura? (Escolha duas)');
select @CodigoPerguntaInserida = MAX(CodigoPergunta) from Pergunta
insert into Resposta (Ativo,CodigoPergunta,Correta,Texto) values
(1,@CodigoPerguntaInserida,0,'Crie um usuário do IAM que tenha permissões para o bucket do S3. Adicione o usuário a um grupo do IAM.'),
(1,@CodigoPerguntaInserida,1,'Crie uma função do IAM que tenha permissões para o bucket do S3.'),
(1,@CodigoPerguntaInserida,1,'Adicione a função do IAM a um perfil de instância. Anexe o perfil de instância à instância do EC2.'),
(1,@CodigoPerguntaInserida,0,'Crie uma função do IAM que tenha permissões para o bucket do S3. Atribua a função a um grupo da 1AM.'),
(1,@CodigoPerguntaInserida,0,'Armazene as credenciais do usuário do IAM nas variáveis ​​de ambiente na instância do EC2.')

--inserindo a prova 2
insert into Prova (Descricao,Ativo) values ('AWS Certified Solutions Architect - Associate',1)

--inserindo a prova 3
insert into Prova (Descricao,Ativo) values ('AWS Certified Solutions Architect - Professional',1)

go

use master;






