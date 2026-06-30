# Ticketing API

## Description
API simples para gestão de tickets, construída com ASP.NET.

Este projeto faz parte de um onboarding challenge e será desenvolvido de forma incremental, adicionando funcionalidades como validação, persistência, autenticação e testes.


## Requirements

- Podman or Docker installed

## Build API Container

podman pod create --name api-pod -p 3000:3000

podman run -d --pod api-pod --name api ticketing-api

## Test

Open:

http://localhost:3000/health


> Note:
> The application runs in Development mode inside the container
> to allow Swagger usage during the learning phase.

---

## Estrutura atual

/Presentation  
  └── Ticketing.Api → Projeto Web API (startup)
/Domain 
  └── Ticketing.Domain
/Application 
  └── Ticketing.Application 
/Infrastructure 
  └── Ticketing.Infrastructure 

---

## Como executar

```bash
dotnet run --project Presentation/Ticketing.Api
```




