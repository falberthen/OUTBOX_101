#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Outbox_101.EventProducer/Outbox_101.EventProducer.csproj", "src/Outbox_101.EventProducer/"]
COPY ["src/Outbox_101.Domain/Outbox_101.Domain.csproj", "src/Outbox_101.Domain/"]
COPY ["src/Outbox_101.Infrastructure.Workers/Outbox_101.Infrastructure.Workers.csproj", "src/Outbox_101.Infrastructure.Workers/"]
COPY ["src/Outbox_101.Infrastructure.Kafka/Outbox_101.Infrastructure.Kafka.csproj", "src/Outbox_101.Infrastructure.Kafka/"]
COPY ["src/Outbox_101.Infrastructure/Outbox_101.Infrastructure.csproj", "src/Outbox_101.Infrastructure/"]
RUN dotnet restore "src/Outbox_101.EventProducer/Outbox_101.EventProducer.csproj"
COPY . .
WORKDIR "/src/src/Outbox_101.EventProducer"
RUN dotnet build "Outbox_101.EventProducer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Outbox_101.EventProducer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Outbox_101.EventProducer.dll"]