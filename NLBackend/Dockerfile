# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todo el proyecto
COPY . .

# Restaura dependencias y publica la app (genera archivos en /app/publish)
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia los archivos publicados desde la etapa anterior
COPY --from=build /app/publish .

# Expone el puerto (ajusta si usas otro)
EXPOSE 80

# Comando de inicio
ENTRYPOINT ["dotnet", "NLBackend.dll"]
