# Ticketing API

## Description

API para gestão de tickets construída com ASP.NET Core, seguindo uma arquitetura em camadas e práticas modernas de desenvolvimento.

Este projeto faz parte de um onboarding challenge e está a ser desenvolvido de forma incremental, evoluindo por etapas:

- Containerização (API + PostgreSQL)
- Integração com base de dados (EF Core)
- Migrations (Code First)
- Validação
- Autenticação
- Testes
---
## Current Features

✅ API containerizada com Podman/Docker  
✅ PostgreSQL configurado em container  
✅ EF Core (Code First) integrado  
✅ Migrations implementadas  
✅ Base de dados criada automaticamente via migrations  
✅ Health check endpoint  
✅ Swagger disponível  

---

## Requirements

- .NET SDK (10.0 ou compatível com o projeto)
- Podman ou Docker
- CLI do EF Core (`dotnet ef`)

---

## Project Structure

/Presentation
└── Ticketing.Api           # Startup project (Web API)
/Application
└── Ticketing.Application   # Application layer (services, logic)
/Domain
└── Ticketing.Domain        # Domain models
/Infrastructure
└── Ticketing.Infrastructure # EF Core, DbContext, repositories, migrations

---

## Running the Project

### Start everything (API + PostgreSQL)

```bash
podman compose up --build
```

--build é necessário só quando há alterações no código da API

```bash
podman compose down
```

# Database
A base de dados PostgreSQL corre num container e usa um volume persistente.

Connection details:

- Host: localhost
- Port: 5432
- User: postgres
- Password: secret
- Database: ticketing

--- 

## Migrations (EF Core)
O projeto usa Code First + Migrations.

### Criar uma migration:
```bash
dotnet ef migrations add <MigrationName> --project .\Infrastructure\Ticketing.Infrastructure\Ticketing.Infrastructure.csproj  --startup-project .\Presentation\Ticketing.Api.csproj --context TicketingDbContext
```

### Aplicar migrations manualmente
```bash

dotnet ef database update --project Infrastructure/Ticketing.Infrastructure --startup-project Presentation/Ticketing.Api --context TicketingDbContext

```

### Aplicação automática no arranque

A API aplica automaticamente migrations no startup:

```bash
db.Database.Migrate();

```

Isto garante:

- criação da base de dados
- aplicação de migrations pendentes
- comportamento idempotente

---

# Development vs Container Environment

O projeto usa diferentes connection strings dependendo do ambiente:
## Tables

| Contexto | Host DB |
|----------|--------|
| Local (dotnet run / migrations) | localhost |
| Container (API → DB) | db |

---

# Accessing the API
## Health Check

http://localhost:3000/health

## Swagger

http://localhost:3000/swagger

Swagger está ativo em todos os ambientes por ser uma API-only (uso controlado esperado).

## How Containers Work

- Containers são recriados a cada compose up
- A imagem só é rebuildada com --build
- A base de dados persiste via volume

# Sem rebuild:
```bash
podman compose up
```

# Com rebuild:

```bash
podman compose up --build
```

# Reset Environment
Para apagar totalmente a base de dados:
```bash
podman compose -down -v
```
Remove volumes → DB será recriada do zero



