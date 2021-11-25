#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NewsTrack.WebApi/NewsTrack.WebApi.csproj", "NewsTrack.WebApi/"]
COPY ["NewsTrack.Data/NewsTrack.Data.csproj", "NewsTrack.Data/"]
COPY ["NewsTrack.Domain/NewsTrack.Domain.csproj", "NewsTrack.Domain/"]
COPY ["NewsTrack.Common/NewsTrack.Common.csproj", "NewsTrack.Common/"]
COPY ["NewsTrack.Identity/NewsTrack.Identity.csproj", "NewsTrack.Identity/"]
COPY ["NewsTrack.Browser/NewsTrack.Browser.csproj", "NewsTrack.Browser/"]
RUN dotnet restore "NewsTrack.WebApi/NewsTrack.WebApi.csproj"
COPY . .
WORKDIR "/src/NewsTrack.WebApi"
RUN dotnet build "NewsTrack.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewsTrack.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsTrack.WebApi.dll"]