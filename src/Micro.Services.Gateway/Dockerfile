FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Micro.Services.Gateway/Micro.Services.Gateway.csproj", "src/Micro.Services.Gateway/"]
RUN dotnet restore "src/Micro.Services.Gateway/Micro.Services.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Micro.Services.Gateway"
RUN dotnet build "Micro.Services.Gateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Micro.Services.Gateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Micro.Services.Gateway.dll"]