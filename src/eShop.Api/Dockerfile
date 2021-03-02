#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/eShop.Api/eShop.Api.csproj", "src/eShop.Api/"]
COPY ["src/eShop.Application/eShop.Application.csproj", "src/eShop.Application/"]
COPY ["src/eShop.Domain/eShop.Domain.csproj", "src/eShop.Domain/"]
COPY ["src/eShop.Infrastructure/eShop.Infrastructure.csproj", "src/eShop.Infrastructure/"]
COPY ["src/eShop.Common/eShop.Common.csproj", "src/eShop.Common/"]
RUN dotnet restore "src/eShop.Api/eShop.Api.csproj"
COPY . .
WORKDIR "/src/src/eShop.Api"
RUN dotnet build "eShop.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eShop.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.Api.dll"]