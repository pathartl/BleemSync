version="0.3.2"

# Must run the following as admin if you want to build and compress
# Install-Module 7Zip4PowerShell -Force -Verbose

rm -rf BleemSync.Payload/System/BleemSync
mkdir -p BleemSync.Payload/System/BleemSync

dotnet publish -c release -r linux-arm
cp -R BleemSync/bin/Release/netcoreapp2.1/linux-arm/publish/* ./BleemSync.Payload/System/BleemSync

dotnet publish -c release -r linux-x64
rm -rf ./Publish
mkdir -p ./Publish/BleemSync
cp ./BleemSync/bin/Release/netcoreapp2.1/linux-x64/publish/* ./Publish/BleemSync
cp -R ./BleemSync.Payload/* ./Publish
7za a BleemSync-$version-linux-x64.zip Publish/*
# Compress-7Zip "Publish/*" -Verbose -ArchiveFileName BleemSync-$version-linux-x64.zip -Format Zip

dotnet publish -c release -r osx-x64
rm -rf ./Publish
mkdir -p Publish/BleemSync
cp ./BleemSync/bin/Release/netcoreapp2.1/osx-x64/publish/* ./Publish/BleemSync
cp -R ./BleemSync.Payload/* ./Publish
7za a BleemSync-$version-osx-x64.zip Publish/*
# Compress-7Zip "Publish" -ArchiveFileName BleemSync-$version-osx-x64.zip -Format Zip

dotnet publish -c release -r win7-x86
rm -rf ./Publish
mkdir -p Publish/BleemSync
cp ./BleemSync/bin/Release/netcoreapp2.1/win7-x86/publish/* ./Publish/BleemSync
cp -R ./BleemSync.Payload/* ./Publish
7za a BleemSync-$version-win7-x86.zip Publish/*
# Compress-7Zip "Publish" -ArchiveFileName BleemSync-$version-win7-x86.zip -Format Zip