FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
WORKDIR /app

ENV PORT=5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dentist-panel-api/dentist-multi-panel-api.csproj", "dentist-panel-api/"]
RUN dotnet restore "dentist-panel-api/dentist-multi-panel-api.csproj"
COPY . .
WORKDIR "/src/dentist-panel-api"
RUN dotnet build "dentist-multi-panel-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dentist-multi-panel-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet dentist-multi-panel-api.dll