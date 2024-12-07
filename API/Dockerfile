# Use the .NET 8 ASP.NET image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Set environment variables for HTTPS
ENV ASPNETCORE_URLS="http://+:5000"
ENV ASPNETCORE_HTTP_PORTS=5000

EXPOSE 5000

# Use the .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the entire API project and other layers from the local machine to the container
COPY ./CalculatorAPI ./CalculatorAPI
COPY ./CalculatorAPI.Business ./CalculatorAPI.Business
COPY ./CalculatorAPI.Data ./CalculatorAPI.Data
COPY ./CalculatorAPI.UnitTests ./CalculatorAPI.UnitTests

# Run the restore command for the CalculatorAPI project
RUN dotnet restore "CalculatorAPI/CalculatorAPI.csproj"

# Copy the rest of the source code
COPY . . 

# Build the API project
WORKDIR "/src/CalculatorAPI"
RUN dotnet build "CalculatorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the API project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CalculatorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage to build the container image with the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalculatorAPI.dll"]