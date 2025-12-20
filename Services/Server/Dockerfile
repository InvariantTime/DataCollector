FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["./Services/Server/src/", "./Services/Server/src/"]
COPY ["Shared/", "./Shared/"]

RUN dotnet publish ./Services/Server/src/DataCollector.Server.API/DataCollector.Server.API.csproj -c Release -o /.out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /.out .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "DataCollector.Server.API.dll"]