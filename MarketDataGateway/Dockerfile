FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MarketDataGateway/MarketDataGateway.csproj", "MarketDataGateway/"]
RUN dotnet restore "MarketDataGateway/MarketDataGateway.csproj"
COPY . .
WORKDIR "/src/MarketDataGateway"
RUN dotnet build "MarketDataGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketDataGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketDataGateway.dll"]
