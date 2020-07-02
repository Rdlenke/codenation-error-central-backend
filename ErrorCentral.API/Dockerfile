#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ErrorCentral.API/ErrorCentral.API.csproj", "ErrorCentral.API/"]
COPY ["ErrorCentral.Infrastructure/ErrorCentral.Infrastructure.csproj", "ErrorCentral.Infrastructure/"]
COPY ["ErrorCentral.Domain/ErrorCentral.Domain.csproj", "ErrorCentral.Domain/"]
RUN dotnet restore "ErrorCentral.API/ErrorCentral.API.csproj"
COPY . .
WORKDIR "/src/ErrorCentral.API"
RUN dotnet build "ErrorCentral.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ErrorCentral.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ErrorCentral.API.dll"]