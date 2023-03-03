#! /bin/bash
@echo off
java -jar generator-cli.jar generate -i specs.yaml -c configs.json -g aspnetcore -o ../
echo Open api generator has been executed!
cd ..
dotnet tool restore 
dotnet jb cleanupcode Domivice.Users.sln
echo All codes now formatted!