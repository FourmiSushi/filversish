#!/bin/sh
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 6.0 -InstallDir ./dotnet6
./dotnet6/dotnet --version

mkdir -p gh-build/linux
mkdir -p gh-build/windows
mkdir -p gh-build/macos

./dotnet6/dotnet publish . -r win-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/windows
./dotnet6/dotnet publish . -r linux-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/linux
./dotnet6/dotnet publish . -r osx-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/macos

mv gh-build/windows/filversish.exe gh-build/windows/filversish_win-x64.exe
mv gh-build/linux/filversish gh-build/linux/filversish_linux-x64
mv gh-build/macos/filversish gh-build/macos/filversish_osx-x64