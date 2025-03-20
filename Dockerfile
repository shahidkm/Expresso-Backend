# Base image for running the app in production or debug mode
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file directly from the root
COPY ["AuthenticationService.csproj", "./"]

# Restore dependencies
RUN dotnet restore "AuthenticationService.csproj"

# Copy the rest of the source code
COPY . .

# Build the app
WORKDIR "/src"
RUN dotnet build "AuthenticationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthenticationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage to run the app in production
FROM base AS final
WORKDIR /app

# Copy the build output from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "AuthenticationService.dll"]
