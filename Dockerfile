# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

COPY . /source
WORKDIR /source

ARG TARGETARCH

# ✅ FIXED: Only one publish command
RUN dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

# ❗ Optional: Remove or define this if needed
# USER $APP_UID

ENTRYPOINT ["dotnet", "CapstoneTeam11.dll"]