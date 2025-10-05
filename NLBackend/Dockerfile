# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el .csproj (ya está dentro del contexto)
COPY NLBackend.csproj ./
RUN dotnet restore NLBackend.csproj

# Copiamos el resto del código
COPY . .
RUN dotnet publish NLBackend.csproj -c Release -o /app

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render inyecta PORT automáticamente
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "NLBackend.dll"]
