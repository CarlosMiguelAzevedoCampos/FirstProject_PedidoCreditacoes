#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CMA.ISMAI.Solutions.Creditacoes.UI/CMA.ISMAI.Solutions.Creditacoes.UI.csproj", "CMA.ISMAI.Solutions.Creditacoes.UI/"]
COPY ["CMA.ISMAI.Core/CMA.ISMAI.Core.csproj", "CMA.ISMAI.Core/"]
COPY ["CMA.ISMAI.Logging/CMA.ISMAI.Logging.csproj", "CMA.ISMAI.Logging/"]
RUN dotnet restore "CMA.ISMAI.Solutions.Creditacoes.UI/CMA.ISMAI.Solutions.Creditacoes.UI.csproj"
COPY . .
WORKDIR "/src/CMA.ISMAI.Solutions.Creditacoes.UI"
RUN dotnet build "CMA.ISMAI.Solutions.Creditacoes.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMA.ISMAI.Solutions.Creditacoes.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMA.ISMAI.Solutions.Creditacoes.UI.dll"]