# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5000

# Etapa de compilación (build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar y restaurar dependencias
COPY ["Uttt.Micro.Service.csproj", "."]
RUN dotnet restore "Uttt.Micro.Service.csproj"

# Copiar todo el código fuente
COPY . ./

# Construir y publicar
RUN dotnet build "Uttt.Micro.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "Uttt.Micro.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build-env /app/publish .

# Configurar puerto y entrypoint
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Uttt.Micro.Service.dll"]