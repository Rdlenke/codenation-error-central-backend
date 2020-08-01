
# Central de Erros

[![Maintainability](https://api.codeclimate.com/v1/badges/2c89f7fac35b639d8ae9/maintainability)](https://codeclimate.com/github/Rdlenke/codenation-error-central-backend/maintainability)
[![codecov](https://codecov.io/gh/Rdlenke/codenation-error-central-backend/branch/master/graph/badge.svg)](https://codecov.io/gh/Rdlenke/codenation-error-central-backend)

## Sobre o Projeto

### 1. Objetivo
Neste projeto vamos implementar um sistema para centralizar registros de erros de aplicações.

### 2. Justificativa
Em projetos modernos é cada vez mais comum o uso de arquiteturas baseadas em serviços ou microsserviços. 
Nestes ambientes complexos, erros podem surgir em diferentes camadas da aplicação (backend, frontend, mobile, desktop) e mesmo em serviços distintos. 
Desta forma, é muito importante que os desenvolvedores possam centralizar todos os registros de erros em um local, de onde podem monitorar e tomar decisões mais acertadas.

### 3. Requisitos

#### API
- Criar endpoints para serem usados pelo frontend da aplicação.
- Criar um endpoint que será usado para gravar os logs de erro em um banco de dados relacional.
- A API deve ser segura, permitindo acesso apenas com um token de autenticação válido.

### 4. Arquitetura

O projeto propõe uma implementação de DDD, disponibilizando sub projetos de classes para as camadas criadas dentro da solução.



#### API

#### Application

#### Domain

#### Infrasctruture

#### UnitTests

### 5. Tecnologias

- [ASP.NET API Versioning](https://github.com/microsoft/aspnet-api-versioning)
- [Swashbuckle (Swagger)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Entity Framework](https://docs.microsoft.com/pt-br/ef/)
- [xUnit](https://xunit.net/)
- [Fluent Assertions](https://fluentassertions.com/)
- [Moq](https://github.com/Moq/moq4/wiki/Quickstart)


## Execução do Projeto

### 1. Migrações do Banco de Dados

Para executar os seguintes comandos, é necessário está na pasta onde a solução se encontra.

Após realizar alterações na configuração da entidade (Ex: UserConfiguration) é necessário criar uma nova migração com os campos alterados, essa ação pode ser realizada com a seguinte instrução:

    dotnet ef migrations add [NomeDaMigracao] -s ErrorCentral.API -p ErrorCentral.Infrastructure/
    
Caso queira remover as migrações que foram geradas é necessário executar o próximo comando (Só é possível remover se elas não estiverem no banco de dados):

    dotnet ef migrations remove -s ErrorCentral.API -p ErrorCentral.Infrastructure
    
Para que as atualizações criadas na migrate sejam feitas no banco de dados execute o seguinte comando:

    dotnet ef database update -s ErrorCentral.API -p ErrorCentral.Infrastructure
    
Gerar um script sql das alterações contidas na migrate:

    dotnet ef migrations script -s ErrorCentral.API
    
#### 2. Execução da API

- Dotnet --- [Url](https://localhost:5001)

      dotnet run -p ErrorCentral.API/ErrorCentral.API.csproj
      
- Docker --- [Url]()


#### 3. Execução dos Testes

- Dotnet

      dotnet test


#### 4. Cobertura dos Testes

- Coverlet

    Entrar na pasta de testes:

        cd ErrorCentral.UnitTests

    Gerar arquivo de cobertura e visualizar resultados:
        
        - Linux:

            dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura

        - Windows:

            dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

    Gerar relatório de cobertura do arquivo gerado:

        reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html