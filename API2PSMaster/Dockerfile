#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["api2psmaster/api2psmaster.csproj", "api2psmaster/"]
RUN dotnet restore "./api2psmaster/API2PSMaster.csproj"
COPY . .
WORKDIR "/src/api2psmaster"
RUN dotnet build "API2PSMaster.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API2PSMaster.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API2PSMaster.dll"]