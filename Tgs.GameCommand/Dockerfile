#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameCommand/Tgs.GameCommand.csproj", "Tgs.GameCommand/"]
COPY ["Tgs.GameCommand.Core/Tgs.GameCommand.Core.csproj", "Tgs.GameCommand.Core/"]
COPY ["Tgs.GameCommand.Common/Tgs.GameCommand.Common.csproj", "Tgs.GameCommand.Common/"]
COPY ["Tgs.GameCommand.Repositories/Tgs.GameCommand.Repositories.csproj", "Tgs.GameCommand.Repositories/"]
RUN dotnet restore "./Tgs.GameCommand/./Tgs.GameCommand.csproj"
COPY . .
WORKDIR "/src/Tgs.GameCommand"
RUN dotnet build "./Tgs.GameCommand.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameCommand.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameCommand.dll"]