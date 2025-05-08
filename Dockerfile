# syntax=docker/dockerfile:1

# --------------------
# Build Stage
# --------------------
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
    WORKDIR /source
    
    COPY . .
    
    RUN dotnet restore
    RUN dotnet publish -c Release -r linux-x64 --self-contained false -o /app
    
    # --------------------
    # Runtime Stage
    # --------------------
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
    WORKDIR /app
    
    COPY --from=build /app .
    
    ENTRYPOINT ["dotnet", "CapstoneTeam11.dll"]