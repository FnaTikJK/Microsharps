FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Запускать из родительской папки всех солюшенов (Докер не может работать с папками выше самого себя)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Сохраняем структуру папок в соответствии с ссылками на проекты в оригинале (чтобы не делать лишних `dotnet remove/add reference`)
WORKDIR /src/Infrastructure
COPY ["Infrastructure/Infrastructure.API/Infrastructure.API.csproj", "Infrastructure.API/"]
COPY ["Infrastructure/Infrastructure.Client/Infrastructure.csproj", "Infrastructure.Client/"]
COPY ["Infrastructure/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
WORKDIR /src/service
COPY ["ServiceDiscovery/ServiceDiscovery.Models/ServiceDiscovery.Models.csproj", "ServiceDiscovery.Models/"]
RUN dotnet restore "Infrastructure.API/Infrastructure.API.csproj"
WORKDIR /src
COPY ./Infrastructure ./Infrastructure
WORKDIR "/src/service/Infrastructure.API"
RUN dotnet build "Infrastructure.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Infrastructure.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Infrastructure.API.dll"]
