FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY . .
WORKDIR /src/src/ServiceB/Micro.ServiceB
RUN dotnet restore
RUN dotnet build --no-restore -c Release -o /app

FROM build as integrationtest
WORKDIR /src/src/ServiceB/Micro.ServiceB.IntegrationTests

FROM build as unittest
WORKDIR /src/src/ServiceB/Micro.ServiceB.UnitTests

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Micro.ServiceB.dll"]