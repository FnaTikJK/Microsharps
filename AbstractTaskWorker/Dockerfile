FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app


# Запускать из родительской папки всех солюшенов (Докер не может работать с папками выше самого себя)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Сохраняем структуру папок в соответствии с ссылками на проекты в оригинале (чтобы не делать лишних `dotnet remove/add reference`)
WORKDIR /src/AbstractTaskService
COPY ["AbstractTaskService/AbstractTaskService.DAL/AbstractTaskService.DAL.csproj", "AbstractTaskService.DAL/"]
WORKDIR /src/service
COPY ["AbstractTaskWorker/AbstractTaskWorker/AbstractTaskWorker.csproj", "AbstractTaskWorker/"]
RUN dotnet restore "AbstractTaskWorker/AbstractTaskWorker.csproj"
WORKDIR /src
COPY ./AbstractTaskService/AbstractTaskService.DAL ./AbstractTaskService/AbstractTaskService.DAL
COPY ./AbstractTaskWorker ./service
WORKDIR "/src/service/AbstractTaskWorker"
RUN dotnet build "AbstractTaskWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AbstractTaskWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbstractTaskWorker.dll"]
