# ========================= BASE =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080
EXPOSE 8081


# ========================= BUILD =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Services/ReminderService/Fundoo.ReminderService.API/Fundoo.ReminderService.API.csproj", "src/Services/ReminderService/Fundoo.ReminderService.API/"]
COPY ["src/Services/ReminderService/Fundoo.ReminderService.Infrastructure/Fundoo.ReminderService.Infrastructure.csproj", "src/Services/ReminderService/Fundoo.ReminderService.Infrastructure/"]
COPY ["src/Services/ReminderService/Fundoo.ReminderService.Application/Fundoo.ReminderService.Application.csproj", "src/Services/ReminderService/Fundoo.ReminderService.Application/"]
COPY ["src/Services/ReminderService/Fundoo.ReminderService.Domain/Fundoo.ReminderService.Domain.csproj", "src/Services/ReminderService/Fundoo.ReminderService.Domain/"]

RUN dotnet restore "./src/Services/ReminderService/Fundoo.ReminderService.API/Fundoo.ReminderService.API.csproj"

COPY . .

WORKDIR "/src/src/Services/ReminderService/Fundoo.ReminderService.API"

RUN dotnet build "Fundoo.ReminderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========================= PUBLISH =========================
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Fundoo.ReminderService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# ========================= FINAL =========================
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Fundoo.ReminderService.API.dll"]