FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CMA.ISMAI.Trello.API/CMA.ISMAI.Trello.API.csproj", "CMA.ISMAI.Trello.API/"]
COPY ["CMA.ISMAI.Trello.Engine/CMA.ISMAI.Trello.Engine.csproj", "CMA.ISMAI.Trello.Engine/"]
COPY ["CMA.ISMAI.Core/CMA.ISMAI.Core.csproj", "CMA.ISMAI.Core/"]
COPY ["CMA.ISMAI.Logging/CMA.ISMAI.Logging.csproj", "CMA.ISMAI.Logging/"]
COPY ["CMA.ISMAI.Trello.Domain/CMA.ISMAI.Trello.Domain.csproj", "CMA.ISMAI.Trello.Domain/"]
COPY ["CMA.ISMAI.Trello.FileReader/CMA.ISMAI.Trello.FileReader.csproj", "CMA.ISMAI.Trello.FileReader/"]
COPY ["CMA.ISMAI.Engine.Automation/CMA.ISMAI.Trello.Engine.Automation.csproj", "CMA.ISMAI.Engine.Automation/"]
RUN dotnet restore "CMA.ISMAI.Trello.API/CMA.ISMAI.Trello.API.csproj"
COPY . .
WORKDIR "/src/CMA.ISMAI.Trello.API"
RUN dotnet build "CMA.ISMAI.Trello.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMA.ISMAI.Trello.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMA.ISMAI.Trello.API.dll"]