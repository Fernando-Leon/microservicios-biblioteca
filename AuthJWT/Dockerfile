FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Tienda.Microservicio.Auth.Api.csproj", "."]
RUN dotnet restore "./Tienda.Microservicio.Auth.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Tienda.Microservicio.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tienda.Microservicio.Auth.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tienda.Microservicio.Auth.Api.dll"]
