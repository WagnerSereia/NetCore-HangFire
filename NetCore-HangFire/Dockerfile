#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NetCore-HangFire/NetCore-HangFire.csproj", "NetCore-HangFire/"]
RUN dotnet restore "NetCore-HangFire/NetCore-HangFire.csproj"
COPY . .
WORKDIR "/src/NetCore-HangFire"
RUN dotnet build "NetCore-HangFire.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore-HangFire.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore-HangFire.dll"]