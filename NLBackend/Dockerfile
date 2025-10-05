# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todo el proyecto (como el original)
COPY . .

# Restaura y publica
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render inyecta PORT; hacemos que Kestrel escuche ahí
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# Copia el output publicado
COPY --from=build /app/publish .

# EXPOSE es informativo; Render hace proxy al $PORT
EXPOSE 8080

# EntryPoint (igual que tu original)
ENTRYPOINT ["dotnet", "NLBackend.dll"]
