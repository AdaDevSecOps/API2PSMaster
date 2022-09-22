#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# WORKDIR /app
# EXPOSE 80

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src
# COPY . .
# #RUN dotnet restore "./API2PSMaster/API2PSMaster.csproj"
# # WORKDIR "/src/API2PSMaster"
# RUN dotnet build "/src/API2PSMaster/API2PSMaster.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "API2PSMaster.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "API2PSMaster.dll"]


FROM mcr.microsoft.com/dotnet/sdk:6.0-windowservercore-ltsc2019 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY API2PSMaster/*.csproj .
RUN dotnet restore --use-current-runtime  