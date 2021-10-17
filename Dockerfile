# First stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /DockerSource

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY TwelveFactor/*.csproj ./TwelveFactor/
RUN dotnet restore

# Copy everything else and build website
COPY TwelveFactor/. ./TwelveFactor/
WORKDIR /DockerSource/TwelveFactor
RUN dotnet publish -c release -o /DockerOutput/Website

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /DockerOutput/Website
COPY --from=build /DockerOutput/Website ./
ENTRYPOINT ["dotnet", "TwelveFactor.dll"]