#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CMA.ISMAI.Sagas/CMA.ISMAI.Sagas.UI.csproj", "CMA.ISMAI.Sagas/"]
COPY ["CMA.ISMAI.Core/CMA.ISMAI.Core.csproj", "CMA.ISMAI.Core/"]
COPY ["CMA.ISMAI.Logging/CMA.ISMAI.Logging.csproj", "CMA.ISMAI.Logging/"]
COPY ["CMA.ISMAI.Sagas.Domain/CMA.ISMAI.Sagas.Domain.csproj", "CMA.ISMAI.Sagas.Domain/"]
COPY ["CMA.ISMAI.Engine.Sagas/CMA.ISMAI.Sagas.Service.csproj", "CMA.ISMAI.Engine.Sagas/"]
RUN dotnet restore "CMA.ISMAI.Sagas/CMA.ISMAI.Sagas.UI.csproj"
COPY . .
WORKDIR "/src/CMA.ISMAI.Sagas"
RUN dotnet build "CMA.ISMAI.Sagas.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMA.ISMAI.Sagas.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMA.ISMAI.Sagas.UI.dll"]