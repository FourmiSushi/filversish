#!/bin/sh
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 8.0 -InstallDir ./dotnet8
./dotnet8/dotnet --version

mkdir -p gh-build/linux
mkdir -p gh-build/windows
mkdir -p gh-build/macos

./dotnet8/dotnet publish filversish -r win-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/windows
./dotnet8/dotnet publish filversish -r linux-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/linux
./dotnet8/dotnet publish filversish -r osx-x64 -c Release --sc -p:PublishSingleFile=true -o gh-build/macos

mv gh-build/windows/filversish.exe gh-build/windows/filversish_win-x64.exe
mv gh-build/linux/filversish gh-build/linux/filversish_linux-x64
mv gh-build/macos/filversish gh-build/macos/filversish_osx-x64

first_tag=$(git tag --sort -creatordate | head -1)
second_tag=$(git tag --sort -creatordate | head -2 | tail -1)

git log --pretty=format:"%h %s by %an" $second_tag..$first_tag > changelog.txt
