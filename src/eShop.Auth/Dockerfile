#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/eShop.Auth/eShop.Auth.csproj", "src/eShop.Auth/"]
RUN dotnet restore "src/eShop.Auth/eShop.Auth.csproj"
COPY . .
WORKDIR "/src/src/eShop.Auth"
RUN dotnet build "eShop.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eShop.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.Auth.dll"]