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
RUN dotnet publish -c release -o /DockerOutput/App --self-contained -r ubuntu.20.04-x64

# Final stage
# https://hub.docker.com/_/microsoft-dotnet-aspnet with tag for Ubuntu 20.04 amd64
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal-amd64
WORKDIR /DockerOutput/App
COPY --from=build /DockerOutput/App ./
ENTRYPOINT ["dotnet", "TwelveFactor.dll"]

# Define environment variables so they can be set during docker build
# An optional default value can be set
ARG upper_case_guids=true
ENV UPPERCASEGUIDS=$upper_case_guids

ARG get_new_guid_delay_ms=2000
ENV GETNEWGUIDDELAYMS=$get_new_guid_delay_ms
