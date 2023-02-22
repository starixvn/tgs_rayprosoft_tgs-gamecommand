#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameCommand/Tgs.GameCommand.csproj", "Tgs.GameCommand/"]
COPY ["Tgs.GameCommand.Core/Tgs.GameCommand.Core.csproj", "Tgs.GameCommand.Core/"]
COPY ["Tgs.GameCommand.Common/Tgs.GameCommand.Common.csproj", "Tgs.GameCommand.Common/"]
COPY ["Tgs.GameCommand.Repositories/Tgs.GameCommand.Repositories.csproj", "Tgs.GameCommand.Repositories/"]
RUN dotnet restore "Tgs.GameCommand/Tgs.GameCommand.csproj"
COPY . .
WORKDIR "/src/Tgs.GameCommand"
RUN dotnet build "Tgs.GameCommand.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameCommand.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameCommand.dll"]