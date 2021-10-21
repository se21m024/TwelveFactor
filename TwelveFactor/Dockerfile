# First stage
# https://hub.docker.com/_/microsoft-dotnet-sdk with tag for Ubuntu 20.04 amd64
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal-amd64 AS build
WORKDIR /DockerSource

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY TwelveFactor/TwelveFactor.csproj ./TwelveFactor/
RUN dotnet restore

# Copy everything else and build website
COPY ./. ./TwelveFactor/
WORKDIR /DockerSource/TwelveFactor
RUN dotnet publish -c release -o /DockerOutput/Website --self-contained -r ubuntu.20.04-x64

# Final stage
# https://hub.docker.com/_/microsoft-dotnet-aspnet with tag for Ubuntu 20.04 amd64
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal-amd64
WORKDIR /DockerOutput/Website
COPY --from=build /DockerOutput/Website ./
ENTRYPOINT ["dotnet", "TwelveFactor.dll"]

# Define environment variables so they can be set during docker build
# An optional default value would be possible: ARG user-name=Tom
ARG user_name=None
ENV USERNAME=$user_name