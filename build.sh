#!/usr/bin/env bash
#
# Generated by: https://openapi-generator.tech
#

dotnet restore src/Domivice.Users.Web/ && \
    dotnet build src/Domivice.Users.Web/ && \
    echo "Now, run the following to start the project: dotnet run -p src/Domivice.Users.Web/Domivice.Users.Web.csproj --launch-profile web"
