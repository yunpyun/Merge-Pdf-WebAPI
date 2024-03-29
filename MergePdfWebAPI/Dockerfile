#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base

RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update; apt-get install -y ttf-mscorefonts-installer fontconfig

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MergePdfWebAPI/MergePdfWebAPI.csproj", "MergePdfWebAPI/"]
RUN dotnet restore "MergePdfWebAPI/MergePdfWebAPI.csproj"
COPY . .
WORKDIR "/src/MergePdfWebAPI"
RUN dotnet build "MergePdfWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MergePdfWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MergePdfWebAPI.dll"]