# syntax=docker/dockerfile:1

# --------------------
# Build Stage
# --------------------
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
    WORKDIR /source
    
    # Copy everything
    COPY . .
    
    # Restore and publish
    RUN dotnet restore
    RUN dotnet publish -c Release -r linux-x64 --self-contained false -o /app
    
    # --------------------
    # Runtime Stage
    # --------------------
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
    WORKDIR /app
    
    # Copy published output from build stage
    COPY --from=build /app .
    
    # Start the app
    ENTRYPOINT ["dotnet", "CapstoneTeam11.dll"]