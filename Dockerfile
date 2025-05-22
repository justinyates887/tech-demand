# Dockerfile (at the repo root)

#######################################
# 1) Runtime base image (8.0)
#######################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#######################################
# 2) SDK image for build (8.0)
#######################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ["TechStax.csproj", "./"]
RUN dotnet restore "./TechStax.csproj"

# copy everything else and publish
COPY . .
RUN dotnet publish "TechStax.csproj" -c Release -o /app/publish

#######################################
# 3) Final smaller runtime image
#######################################
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechStax.dll"]
