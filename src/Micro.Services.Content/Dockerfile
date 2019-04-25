FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /build
COPY . .
WORKDIR /build/src/Micro.Services.Content
RUN dotnet restore
RUN dotnet build --no-restore -c Release -o /app

FROM build as unit
WORKDIR /build/tests/Micro.Services.Content.UnitTests

FROM build as integration
WORKDIR /build/tests/Micro.Services.Content.IntegrationTests

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Micro.Services.Content.dll"]