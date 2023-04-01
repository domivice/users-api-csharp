#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5005

# Build container
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
ARG PACKAGE_SOURCE_USERNAME
ARG PACKAGE_SOURCE_PASSWORD

# Copy the code into the container
COPY Domivice.Users.sln .
COPY src/Domivice.Users.Application/*.csproj ./src/Domivice.Users.Application/
COPY src/Domivice.Users.Domain/*.csproj ./src/Domivice.Users.Domain/
COPY src/Domivice.Users.Infrastructure/*.csproj ./src/Domivice.Users.Infrastructure/
COPY src/Domivice.Users.Web/*.csproj ./src/Domivice.Users.Web/
COPY tests/Domivice.Users.Web.Tests/*.csproj ./tests/Domivice.Users.Web.Tests/

# NuGet restore
RUN dotnet nuget add source https://pkgs.dev.azure.com/domivice/Domivice/_packaging/DomiviceNugets/nuget/v3/index.json -u $PACKAGE_SOURCE_USERNAME -p $PACKAGE_SOURCE_PASSWORD --store-password-in-clear-text
RUN dotnet restore

COPY src/Domivice.Users.Web/. ./src/Domivice.Users.Web/
COPY tests/Domivice.Users.Web.Tests/. ./src/Domivice.Users.Web.Tests/
COPY src/Domivice.Users.Application/. ./src/Domivice.Users.Application/
COPY src/Domivice.Users.Domain/. ./src/Domivice.Users.Domain/
COPY src/Domivice.Users.Infrastructure/. ./src/Domivice.Users.Infrastructure/


# Build the API
WORKDIR src/Domivice.Users.Web
RUN dotnet build "Domivice.Users.Web.csproj" -c Release -o /app/build

# Publish it
FROM build AS publish
RUN dotnet publish "Domivice.Users.Web.csproj" -c Release -o /app/publish

# Make the final image for publishing
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Domivice.Users.Web.dll"]
