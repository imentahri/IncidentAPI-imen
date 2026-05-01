FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /api

COPY IncidentAPI-imen.slnx ./
COPY IncidentAPI-imen/*.csproj IncidentAPI-imen/
COPY AppTests/*.csproj AppTests/

RUN dotnet restore IncidentAPI-imen.slnx

COPY . .

RUN dotnet publish IncidentAPI-imen/IncidentAPI-imen.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IncidentAPI-imen.dll"]