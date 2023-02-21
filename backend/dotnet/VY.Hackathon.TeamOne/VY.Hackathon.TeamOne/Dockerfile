#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VY.Hackathon.TeamOne.WebApi.csproj", "VY.Hackathon.TeamOne/"]
COPY ["VY.Hackaton.Entities.csproj", "VY.Hackaton.Entities/"]
COPY ["VY.Hackaton.TeamOne.ProviderData.Infrastructure.csproj", "VY.Hackaton.TeamOne.ProviderData.Infrastructure/"]
RUN dotnet restore "VY.Hackathon.TeamOne/VY.Hackathon.TeamOne.csproj"
WORKDIR "/src/VY.Hackathon.TeamOne"
COPY . .
RUN dotnet build "VY.Hackathon.TeamOne.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VY.Hackathon.TeamOne.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VY.Hackathon.TeamOne.dll"]