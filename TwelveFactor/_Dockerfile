FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TwelveFactor/TwelveFactor.csproj", "TwelveFactor/"]
RUN dotnet restore "TwelveFactor/TwelveFactor.csproj"
COPY . .
WORKDIR "/src/TwelveFactor"
RUN dotnet build "TwelveFactor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwelveFactor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwelveFactor.dll"]
