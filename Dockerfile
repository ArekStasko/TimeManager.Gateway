#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TimeManager.Gateway/TimeManager.Gateway.csproj", "TimeManager.Gateway/"]
RUN dotnet restore "TimeManager.Gateway/TimeManager.Gateway.csproj"
COPY . .
WORKDIR "/src/TimeManager.Gateway"
RUN dotnet build "TimeManager.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeManager.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeManager.Gateway.dll"]