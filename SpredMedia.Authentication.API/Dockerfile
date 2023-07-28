#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpredMedia.Authentication.API/SpredMedia.Authentication.API.csproj", "SpredMedia.Authentication.API/"]
COPY ["SpredMedia.Authentication.Core/SpredMedia.Authentication.Core.csproj", "SpredMedia.Authentication.Core/"]
COPY ["SpredMedia.Authentication.Infrastructure/SpredMedia.Authentication.Infrastructure.csproj", "SpredMedia.Authentication.Infrastructure/"]
COPY ["SpredMedia.Authentication.Model/SpredMedia.Authentication.Model.csproj", "SpredMedia.Authentication.Model/"]
COPY ["SpredMedia.CommonLibrary/SpredMedia.CommonLibrary.csproj", "SpredMedia.CommonLibrary/"]
RUN dotnet restore "SpredMedia.Authentication.API/SpredMedia.Authentication.API.csproj"
RUN dotnet restore "SpredMedia.Authentication.Core/SpredMedia.Authentication.Core.csproj"
RUN dotnet restore "SpredMedia.Authentication.Infrastructure/SpredMedia.Authentication.Infrastructure.csproj"
RUN dotnet restore "SpredMedia.Authentication.Model/SpredMedia.Authentication.Model.csproj"
RUN dotnet restore "SpredMedia.CommonLibrary/SpredMedia.CommonLibrary.csproj"
COPY . .
WORKDIR "/src/SpredMedia.Authentication.API"
RUN dotnet build "SpredMedia.Authentication.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpredMedia.Authentication.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpredMedia.Authentication.API.dll"]
