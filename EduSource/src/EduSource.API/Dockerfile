# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/EduSource.API/EduSource.API.csproj", "src/EduSource.API/"]
COPY ["src/EduSource.Application/EduSource.Application.csproj", "src/EduSource.Application/"]
COPY ["src/EduSource.Contract/EduSource.Contract.csproj", "src/EduSource.Contract/"]
COPY ["src/EduSource.Domain/EduSource.Domain.csproj", "src/EduSource.Domain/"]
COPY ["src/EduSource.Persistance/EduSource.Persistance.csproj", "src/EduSource.Persistance/"]
COPY ["src/EduSource.Infrastructure.Dapper/EduSource.Infrastructure.Dapper.csproj", "src/EduSource.Infrastructure.Dapper/"]
COPY ["src/EduSource.Infrastructure/EduSource.Infrastructure.csproj", "src/EduSource.Infrastructure/"]
COPY ["src/EduSource.Presentation/EduSource.Presentation.csproj", "src/EduSource.Presentation/"]
RUN dotnet restore "./src/EduSource.API/EduSource.API.csproj"
COPY . .
WORKDIR "/src/src/EduSource.API"
RUN dotnet build "./EduSource.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EduSource.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduSource.API.dll"]