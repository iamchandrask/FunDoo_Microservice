# ========================= BASE IMAGE (RUNTIME) =========================
# This stage defines the runtime environment for the application.
# It contains only the ASP.NET runtime (lightweight).

# ⚠️ IMPORTANT FIX:
# Replaced Windows image (nanoserver) with Linux image for Docker compatibility.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Sets working directory inside container
WORKDIR /app

# Expose ports used by the application
# 8080 → main HTTP port
# 8081 → optional (HTTPS / secondary endpoint if configured)
EXPOSE 8080
EXPOSE 8081


# ========================= BUILD STAGE =========================
# This stage uses full .NET SDK to build the application

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Build configuration (Release by default)
ARG BUILD_CONFIGURATION=Release

# Set working directory
WORKDIR /src

# Copy project files (csproj) first
# This helps Docker cache dependencies (faster builds)
COPY ["src/Services/UserService/Fundoo.UserService.API/Fundoo.UserService.API.csproj", "src/Services/UserService/Fundoo.UserService.API/"]
COPY ["src/Services/UserService/Fundoo.UserService.Application/Fundoo.UserService.Application.csproj", "src/Services/UserService/Fundoo.UserService.Application/"]
COPY ["src/Services/UserService/Fundoo.UserService.Domain/Fundoo.UserService.Domain.csproj", "src/Services/UserService/Fundoo.UserService.Domain/"]
COPY ["src/Services/UserService/Fundoo.UserService.Infrastructure/Fundoo.UserService.Infrastructure.csproj", "src/Services/UserService/Fundoo.UserService.Infrastructure/"]
COPY ["src/BuildingBlocks/Shared.Contracts/Shared.Contracts.csproj", "src/BuildingBlocks/Shared.Contracts/"]

# Restore NuGet packages
RUN dotnet restore "./src/Services/UserService/Fundoo.UserService.API/Fundoo.UserService.API.csproj"

# Copy remaining source code
COPY . .

# Navigate to API project folder
WORKDIR "/src/src/Services/UserService/Fundoo.UserService.API"

# Build the application
# ⚠️ FIX: Linux uses $VAR instead of %VAR%
RUN dotnet build "./Fundoo.UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========================= PUBLISH STAGE =========================
# Publishes compiled app into optimized output

FROM build AS publish

ARG BUILD_CONFIGURATION=Release

# Publish trims unnecessary files and prepares app for runtime
RUN dotnet publish "./Fundoo.UserService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# ========================= FINAL STAGE =========================
# This is the actual container that will run in production

FROM base AS final

# Set working directory
WORKDIR /app

# Copy published files from previous stage
COPY --from=publish /app/publish .

# Entry point: runs the application
ENTRYPOINT ["dotnet", "Fundoo.UserService.API.dll"]