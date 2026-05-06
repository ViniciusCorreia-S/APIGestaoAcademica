# Sistema de Gestao Academica

Projeto academico simples em ASP.NET Core Web API (.NET 10) para gerenciamento academico.

## Tema

Sistema de Gestao Academica com cadastro de alunos, cursos, disciplinas e matriculas em disciplinas.

## Estrutura

- `Controllers`: endpoints REST da API.
- `Models`: entidades persistidas no banco.
- `DTOs`: objetos usados na entrada e saida dos endpoints.
- `Data`: `AppDbContext` do Entity Framework Core.
- `Repositories`: acesso ao banco de dados.
- `Services`: regras de negocio e validacoes.
- `wwwroot`: interface web responsiva com Bootstrap.
- `Migrations`: migration inicial do banco.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- JWT Bearer
- Swagger / Swashbuckle
- Bootstrap

## Banco de dados

Configure a string de conexao em `appsettings.json`:

```json
"ConnectionStrings": {
  "ConexaoPadrao": "server=localhost;port=3306;database=gestao_academica;user=root;password=123456"
}
```

Depois execute:

```powershell
dotnet ef database update
```

## Execucao

```powershell
dotnet run
```

Ao iniciar, acesse:

- Swagger: `https://localhost:7216/swagger`
- Interface web: `https://localhost:7216/index.html`

Se a porta mudar, veja o endereco mostrado no terminal ou em `Properties/launchSettings.json`.

## Usuarios de teste

| Usuario | Senha | Perfil |
| --- | --- | --- |
| `admin` | `123456` | Administrador |
| `secretaria` | `123456` | Secretaria |

## Regras de autorizacao

- `GET /api/alunos`: publico.
- `POST /api/alunos`: Administrador ou Secretaria.
- `PUT /api/alunos/{id}`: Administrador ou Secretaria.
- `DELETE /api/alunos/{id}`: somente Administrador.
- `POST /api/cursos`: somente Administrador.
- `POST /api/matriculas`: Administrador ou Secretaria.
- `DELETE /api/matriculas/{id}`: somente Administrador.

## Testes no Postman

1. Envie `POST /api/auth/login` com `admin` e `123456`.
2. Copie o token retornado.
3. No Postman, use Authorization tipo `Bearer Token`.
4. Teste os endpoints `GET`, `POST`, `PUT` e `DELETE` de alunos.
5. Confirme que exclusao funciona com `admin` e retorna `403 Forbidden` com `secretaria`.

O arquivo `GestaoAcademica.http` tambem contem exemplos prontos de chamadas.
