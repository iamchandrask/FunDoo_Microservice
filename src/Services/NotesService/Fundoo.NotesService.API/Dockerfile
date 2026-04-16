# ========================= BASE IMAGE (RUNTIME) =========================
# This stage is used to run the application (lightweight runtime only)

# ⚠️ FIX:
# Switched from Windows image (nanoserver) → Linux image (compatible with Docker)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set working directory inside container
WORKDIR /app

# Expose ports used by the service
EXPOSE 8080
EXPOSE 8081


# ========================= BUILD STAGE =========================
# This stage uses full .NET SDK to build the application

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Build configuration (Release by default)
ARG BUILD_CONFIGURATION=Release

# Set working directory
WORKDIR /src

# Copy only project files first (for Docker caching optimization)
COPY ["src/Services/NotesService/Fundoo.NotesService.API/Fundoo.NotesService.API.csproj", "src/Services/NotesService/Fundoo.NotesService.API/"]
COPY ["src/BuildingBlocks/Shared.Contracts/Shared.Contracts.csproj", "src/BuildingBlocks/Shared.Contracts/"]
COPY ["src/Services/NotesService/Fundoo.NotesService.Infrastructure/Fundoo.NotesService.Infrastructure.csproj", "src/Services/NotesService/Fundoo.NotesService.Infrastructure/"]
COPY ["src/Services/NotesService/Fundoo.NotesService.Application/Fundoo.NotesService.Application.csproj", "src/Services/NotesService/Fundoo.NotesService.Application/"]
COPY ["src/Services/NotesService/Fundoo.NotesService.Domain/Fundoo.NotesService.Domain.csproj", "src/Services/NotesService/Fundoo.NotesService.Domain/"]

# Restore NuGet packages
RUN dotnet restore "./src/Services/NotesService/Fundoo.NotesService.API/Fundoo.NotesService.API.csproj"

# Copy full source code
COPY . .

# Move to API project directory
WORKDIR "/src/src/Services/NotesService/Fundoo.NotesService.API"

# Build the application
# ⚠️ FIX: Use $BUILD_CONFIGURATION (Linux syntax), not %BUILD_CONFIGURATION%
RUN dotnet build "./Fundoo.NotesService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========================= PUBLISH STAGE =========================
# Publishes optimized output for runtime

FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Fundoo.NotesService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# ========================= FINAL STAGE =========================
# Final runtime container (small + secure)

FROM base AS final

WORKDIR /app

# Copy published output from build stage
COPY --from=publish /app/publish .

# Entry point to run application
ENTRYPOINT ["dotnet", "Fundoo.NotesService.API.dll"]