# Sistema de Gestao Academica

API REST em ASP.NET Core para gerenciamento academico, com cadastro de alunos, cursos, disciplinas e matriculas. O projeto tambem inclui uma interface web simples em Bootstrap servida pela propria API.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- MySQL
- JWT Bearer
- Swagger / Swashbuckle
- Bootstrap

## Estrutura do projeto

```text
APIGestaoAcademica/
|-- APIGestaoAcademica.slnx
|-- README.md
`-- APIGestaoAcademica/
    |-- Controllers/
    |-- Data/
    |-- DTOs/
    |-- Migrations/
    |-- Models/
    |-- Repositories/
    |-- Services/
    |-- wwwroot/
    |-- Program.cs
    `-- APIGestaoAcademica.csproj
```

## Pre-requisitos

Antes de rodar, instale:

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- Git

Opcional, mas recomendado:

- Visual Studio 2026, Visual Studio Code ou Rider
- MySQL Workbench

## Como rodar o projeto

### 1. Clonar o repositorio

```powershell
git clone <url-do-repositorio>
cd APIGestaoAcademica
```

### 2. Conferir a string de conexao

Abra o arquivo:

```text
APIGestaoAcademica/appsettings.Development.json
```

Configure a conexao com o seu MySQL:

```json
{
  "ConnectionStrings": {
    "ConexaoPadrao": "server=localhost;database=gestao_academicaDB;user=root;password=sua_senha"
  }
}
```

Se o seu MySQL usar outra porta, inclua `port=3306` ou a porta correta:

```json
"ConexaoPadrao": "server=localhost;port=3306;database=gestao_academicaDB;user=root;password=sua_senha"
```

### 3. Restaurar os pacotes

Na raiz do repositorio, execute:

```powershell
dotnet restore .\APIGestaoAcademica\APIGestaoAcademica.csproj
```

### 4. Instalar a ferramenta do Entity Framework, se necessario

Se o comando `dotnet ef` ainda nao existir na sua maquina, instale:

```powershell
dotnet tool install --global dotnet-ef
```

Depois, confira:

```powershell
dotnet ef --version
```

### 5. Criar o banco com as migrations

Com o MySQL rodando, execute:

```powershell
dotnet ef database update --project .\APIGestaoAcademica\APIGestaoAcademica.csproj
```

Esse comando cria o banco `gestao_academicaDB` e aplica os dados iniciais de cursos, disciplinas, alunos e matriculas.

### 6. Rodar a aplicacao

```powershell
dotnet run --project .\APIGestaoAcademica\APIGestaoAcademica.csproj --launch-profile https
```

Ao iniciar, a API deve mostrar no terminal os enderecos:

```text
https://localhost:7198
http://localhost:5201
```

## Acessos

- Interface web: `https://localhost:7198/index.html`
- Swagger: `https://localhost:7198/swagger`
- API base: `https://localhost:7198/api`

Se a porta for diferente na sua maquina, confira o arquivo:

```text
APIGestaoAcademica/Properties/launchSettings.json
```

## Usuarios de teste

| Usuario | Senha | Perfil |
| --- | --- | --- |
| `admin` | `123456` | Administrador |
| `secretaria` | `123456` | Secretaria |

Na interface web, use um desses usuarios para fazer login e liberar as operacoes protegidas.

## Endpoints principais

| Metodo | Rota | Autorizacao |
| --- | --- | --- |
| `POST` | `/api/auth/login` | Publico |
| `GET` | `/api/cursos` | Publico |
| `GET` | `/api/cursos/{id}` | Publico |
| `POST` | `/api/cursos` | Administrador |
| `PUT` | `/api/cursos/{id}` | Administrador |
| `DELETE` | `/api/cursos/{id}` | Administrador |
| `GET` | `/api/disciplinas` | Publico |
| `GET` | `/api/disciplinas/{id}` | Publico |
| `POST` | `/api/disciplinas` | Administrador |
| `PUT` | `/api/disciplinas/{id}` | Administrador |
| `DELETE` | `/api/disciplinas/{id}` | Administrador |
| `GET` | `/api/alunos` | Administrador ou Secretaria |
| `GET` | `/api/alunos/{id}` | Administrador ou Secretaria |
| `POST` | `/api/alunos` | Administrador ou Secretaria |
| `PUT` | `/api/alunos/{id}` | Administrador ou Secretaria |
| `DELETE` | `/api/alunos/{id}` | Administrador |
| `GET` | `/api/matriculas` | Administrador ou Secretaria |
| `POST` | `/api/matriculas` | Administrador ou Secretaria |
| `DELETE` | `/api/matriculas/{id}` | Administrador |

## Como autenticar pelo Swagger

1. Acesse `https://localhost:7198/swagger`.
2. Execute `POST /api/auth/login` com:

```json
{
  "usuario": "admin",
  "senha": "123456"
}
```

3. Copie o valor retornado em `token`.
4. Clique em `Authorize`.
5. Informe o token no campo Bearer.

## Problemas comuns

### Erro de conexao com MySQL

Confira se:

- O MySQL esta rodando.
- Usuario e senha estao corretos em `appsettings.Development.json`.
- A porta do MySQL esta correta.
- O usuario informado tem permissao para criar banco e tabelas.

### Comando `dotnet ef` nao encontrado

Instale a ferramenta:

```powershell
dotnet tool install --global dotnet-ef
```

Se ja estiver instalada, atualize:

```powershell
dotnet tool update --global dotnet-ef
```

## Observacoes

- O arquivo `wwwroot/index.html` e a interface web do projeto.
- O Swagger fica disponivel apenas em ambiente de desenvolvimento.
- As credenciais e a chave JWT atuais sao apenas para estudo e desenvolvimento local.
