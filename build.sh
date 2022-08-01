#!/bin/bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 6.0 -InstallDir ./dotnet6
./dotnet6/dotnet --version

mkdir -p gh-build/linux
mkdir -p gh-build/windows
mkdir -p gh-build/macos

./dotnet6/dotnet publish . -r linux-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/linux
./dotnet6/dotnet publish . -r win-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/windows
./dotnet6/dotnet publish . -r osx-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/macos
