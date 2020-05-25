FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /CMA.ISMAI.Trello.API
COPY ["CMA.ISMAI.Trello.API.csproj", "CMA.ISMAI.Trello.API/CMA.ISMAI.Trello.API/"]
COPY ["CMA.ISMAI.Trello.Logging/CMA.ISMAI.Trello.Logging.csproj", "CMA.ISMAI.Trello.Logging/CMA.ISMAI.Trello.Logging/"]
COPY ["CMA.ISMAI.Trello.Domain/CMA.ISMAI.Trello.Domain.csproj", "CMA.ISMAI.Trello.Domain/CMA.ISMAI.Trello.Domain/"]
COPY ["CMA.ISMAI.Trello.Engine/CMA.ISMAI.Trello.Engine.csproj", "CMA.ISMAI.Trello.Engine/CMA.ISMAI.Trello.Engine/"]
COPY ["CMA.ISMAI.Trello.Automation/CMA.ISMAI.Trello.Engine.Automation.csproj", "CMA.ISMAI.Trello.Automation/CMA.ISMAI.Trello.Automation/"]
RUN dotnet restore "CMA.ISMAI.Trello.API/CMA.ISMAI.Trello.API/CMA.ISMAI.Trello.API.csproj"
COPY . .
WORKDIR "/CMA.ISMAI.Trello.API/CMA.ISMAI.Trello.API"
RUN dotnet build "CMA.ISMAI.Trello.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMA.ISMAI.Trello.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMA.ISMAI.Trello.API.dll"]
