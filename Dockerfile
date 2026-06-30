# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copiar solution
COPY Ticketing.slnx ./

# copiar csproj individuais (para cache)
COPY Application/Ticketing.Application/Ticketing.Application.csproj Application/Ticketing.Application/
COPY Domain/Ticketing.Domain/Ticketing.Domain.csproj Domain/Ticketing.Domain/
COPY Infrastructure/Ticketing.Infrastructure/Ticketing.Infrastructure.csproj Infrastructure/Ticketing.Infrastructure/
COPY Presentation/Ticketing.Api.csproj Presentation/

# restore
RUN dotnet restore Presentation/Ticketing.Api.csproj

# copiar resto do código
COPY . .

# publish da API (startup project)
RUN dotnet publish Presentation/Ticketing.Api.csproj -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 3000
ENV ASPNETCORE_URLS=http://+:3000
# remover para versão de produção - swagger só existe em DEV
ENV ASPNETCORE_ENVIRONMENT=Development 


ENTRYPOINT ["dotnet", "Ticketing.Api.dll"]