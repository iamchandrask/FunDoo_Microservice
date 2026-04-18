# ========================= BASE (RUNTIME) =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set working directory
WORKDIR /app

# Expose ports
EXPOSE 8080
EXPOSE 8081


# ========================= BUILD =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Build configuration
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy project files (improves caching)
COPY ["src/Services/CollaborationService/Fundoo.CollaborationService.API/Fundoo.CollaborationService.API.csproj", "src/Services/CollaborationService/Fundoo.CollaborationService.API/"]
COPY ["src/Services/CollaborationService/Fundoo.CollaborationService.Infrastructure/Fundoo.CollaborationService.Infrastructure.csproj", "src/Services/CollaborationService/Fundoo.CollaborationService.Infrastructure/"]
COPY ["src/Services/CollaborationService/Fundoo.CollaborationService.Application/Fundoo.CollaborationService.Application.csproj", "src/Services/CollaborationService/Fundoo.CollaborationService.Application/"]
COPY ["src/Services/CollaborationService/Fundoo.CollaborationService.Domain/Fundoo.CollaborationService.Domain.csproj", "src/Services/CollaborationService/Fundoo.CollaborationService.Domain/"]

# Restore dependencies
RUN dotnet restore "./src/Services/CollaborationService/Fundoo.CollaborationService.API/Fundoo.CollaborationService.API.csproj"

# Copy full source
COPY . .

# Move to API project
WORKDIR "/src/src/Services/CollaborationService/Fundoo.CollaborationService.API"

# FIX: Use Linux variable syntax
RUN dotnet build "Fundoo.CollaborationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========================= PUBLISH =========================
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Fundoo.CollaborationService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# ========================= FINAL =========================
FROM base AS final

WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Run app
ENTRYPOINT ["dotnet", "Fundoo.CollaborationService.API.dll"]