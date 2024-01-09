# Projeto de Autenticação JWT com Idendity

Esse projeto foi desenvolvido em .net 6.0 utilizando a biblioteca nativa Idendity para autenticação.

As principais tecnologias e conceitos utilizados foram:

 - .net 6.0
 - Identity
 - Jwt
 - Roles
 - Claims
 - Confimação de email
 - Migrations
 - Mysql
 - Arquitetura separada por subprojetos

## Rodar Projeto

### `appsettings.json`

- atualizar a `ConnectionStrings:DefaultConnection`  para um banco Mysql
- atualizar o `EmailConfig` para um servidor smpt com usuário e senha valido

### `Migrations`

para subir o banco é nescessário roda o comando `dotnet ef database update` na pasta `AuthJwt.Api`

### `User: admin@admin.com Senha: @dmin123`

Usuario Admin com Role Admin disponível para utilizar as rotas do controller Adm

## Rotas

### `Controller Adm`

Apenas usuários com Role `Admin` logados podem fazer requisições

### `Controller User`

Rotas que precisam de `Role` estão identificadas nas rodas


`Obs` Apenas usuários com email confirmado podem realizar login.