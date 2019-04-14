FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /build
COPY . .
WORKDIR /build/tests/Micro.AcceptanceTests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]