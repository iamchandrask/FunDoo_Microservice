# ========================= BASE IMAGE (RUNTIME) =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# ========================= BUILD STAGE =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files (for caching optimization)
COPY ["src/Services/LabelService/Fundoo.LabelService.API/Fundoo.LabelService.API.csproj", "src/Services/LabelService/Fundoo.LabelService.API/"]
COPY ["src/BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/BuildingBlocks/Shared.Infrastructure/"]
COPY ["src/Services/LabelService/Fundoo.LabelService.Infrastructure/Fundoo.LabelService.Infrastructure.csproj", "src/Services/LabelService/Fundoo.LabelService.Infrastructure/"]
COPY ["src/Services/LabelService/Fundoo.LabelService.Application/Fundoo.LabelService.Application.csproj", "src/Services/LabelService/Fundoo.LabelService.Application/"]
COPY ["src/Services/LabelService/Fundoo.LabelService.Domain/Fundoo.LabelService.Domain.csproj", "src/Services/LabelService/Fundoo.LabelService.Domain/"]

# Restore dependencies
RUN dotnet restore "./src/Services/LabelService/Fundoo.LabelService.API/Fundoo.LabelService.API.csproj"

# Copy full source
COPY . .

# Move to API directory
WORKDIR "/src/src/Services/LabelService/Fundoo.LabelService.API"

# ⚠️ FIX: Linux uses $ not %
RUN dotnet build "./Fundoo.LabelService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========================= PUBLISH STAGE =========================
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Fundoo.LabelService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# ========================= FINAL STAGE =========================
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Fundoo.LabelService.API.dll"]