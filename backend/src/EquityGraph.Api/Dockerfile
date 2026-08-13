# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies (for layer caching)
COPY EquityGraph.Api.csproj ./
RUN dotnet restore EquityGraph.Api.csproj

# Copy source code and publish release build
COPY . ./
RUN dotnet publish EquityGraph.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published artifacts from build stage
COPY --from=build /app/publish ./

# Set default URL and expose port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Entrypoint for the application
ENTRYPOINT ["dotnet", "EquityGraph.Api.dll"]
