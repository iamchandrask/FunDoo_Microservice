# ========================= BASE IMAGE (RUNTIME) =========================
# This stage defines the runtime environment for the API Gateway

# ⚠️ FIX:
# Replaced Windows image (nanoserver) with Linux image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set working directory inside container
WORKDIR /app

# Expose ports used by API Gateway
EXPOSE 8080
EXPOSE 8081


# ========================= BUILD STAGE =========================
# This stage uses full .NET SDK to build the application

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Build configuration (default = Release)
ARG BUILD_CONFIGURATION=Release

# Set working directory
WORKDIR /src

# Copy project file first (for Docker caching optimization)
COPY ["src/ApiGateway/Fundoo.ApiGateway/Fundoo.ApiGateway.csproj", "src/ApiGateway/Fundoo.ApiGateway/"]

# Restore dependencies
RUN dotnet restore "./src/ApiGateway/Fundoo.ApiGateway/Fundoo.ApiGateway.csproj"

# Copy full source code
COPY . .

# Navigate to project folder
WORKDIR "/src/src/ApiGateway/Fundoo.ApiGateway"

# Build the project
# ⚠️ FIX: Use Linux variable syntax ($BUILD_CONFIGURATION)
RUN dotnet build "./Fundoo.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========================= PUBLISH STAGE =========================
# Publishes optimized output for runtime

FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Fundoo.ApiGateway.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# ========================= FINAL STAGE =========================
# Final container used to run the application

FROM base AS final

WORKDIR /app

# Copy published files from build stage
COPY --from=publish /app/publish .

# Start the API Gateway
ENTRYPOINT ["dotnet", "Fundoo.ApiGateway.dll"]