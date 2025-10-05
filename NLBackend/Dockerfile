# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos solo el csproj primero para aprovechar la cache
COPY NLBackend.csproj ./
RUN dotnet restore NLBackend.csproj

# Ahora copiamos el resto del código
COPY . .
RUN dotnet publish NLBackend.csproj -c Release -o /app

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render inyecta PORT; forzamos a escuchar en ese puerto vía env
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
# (Opcional) Asegura entorno de producción
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app ./

# EXPOSE es informativo; 8080 es el valor típico, PORT real lo pone Render
EXPOSE 8080

ENTRYPOINT ["dotnet", "NLBackend.dll"]
