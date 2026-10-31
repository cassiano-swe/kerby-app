FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/kerby.api/kerby.api.csproj src/kerby.api/
RUN dotnet restore src/kerby.api/kerby.api.csproj

COPY src/kerby.api/ src/kerby.api/
RUN dotnet publish src/kerby.api/kerby.api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
USER $APP_UID

ENTRYPOINT ["dotnet", "kerby.api.dll"]
