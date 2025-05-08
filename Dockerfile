# syntax=docker/dockerfile:1

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /source

# Copy source code
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish app (use Release config and correct runtime)
RUN dotnet publish -c Release -r linux-x64 --self-contained false -o /app

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final

WORKDIR /app

# Copy from build
COPY --from=build /app .

# Run the app
ENTRYPOINT ["dotnet", "CapstoneTeam11.dll"]