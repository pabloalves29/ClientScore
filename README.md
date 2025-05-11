O Arquivo clientesdb_clientes.sql é um arquivo DUMP com as informações referentes a base de dados. Recomendo utilizar a ferramenta Workbench (https://www.mysql.com/products/workbench/) para rodar os script
Já o arquivo scriptsCriacoa.sql são os scripts referentes a criação do banco de dados, assim como sua estrutura

A solução do Projetos encontra-se localizada na no caminho: GlobalAccount.API->GlobalAccount.sln

O Projeto foi desenvolvido utilizando .NET 8 e para utilizar os Endpoints desenvolvidos, é necessário realizar a autenticação por meio do Bearer Token. O Token é gerado por meio da rota api/Auth/token 
(as credenciais de usuário e senha estão localizadas no AppSettings do projeto).

As rotas existentes no projeto são:


POST -> api/Client -> responsável pelo cadastro do Cliente, assim como é responsável pelo processo de identificação do score do cliente
PUT -> api/Client -> responsável por efetuar a alteração de alguma informação do cliente caso necessário
GET -> api/Client -> responável por listar todos os clientes cadastrados, retornando seu NOME,SCORE e a Classificação
GET -> api/Client/CPF -> responsável por realizar a pesquisa de cliente por CPF
DELETE -> api/Client/CPF -> responsável por remover o registo do cliente da base
