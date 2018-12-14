$version = "0.3.2"

dotnet publish -c release -r linux-x64
rm -Recurse -Force .\Publish
mkdir .\Publish
mkdir .\Publish\BleemSync
cp .\BleemSync\bin\Release\netcoreapp2.1\linux-x64\publish\* .\Publish\BleemSync
cp -Recurse .\BleemSync.Payload\* .\Publish
Compress-Archive -Force -Path .\Publish\* -CompressionLevel Optimal -DestinationPath .\BleemSync-$version-linux-x64.zip

dotnet publish -c release -r osx-x64
rm -Recurse -Force .\Publish
mkdir .\Publish
mkdir .\Publish\BleemSync
cp .\BleemSync\bin\Release\netcoreapp2.1\osx-x64\publish\* .\Publish\BleemSync
cp -Recurse .\BleemSync.Payload\* .\Publish
Compress-Archive -Force -Path .\Publish\* -CompressionLevel Optimal -DestinationPath .\BleemSync-$version-osx-x64.zip

dotnet publish -c release -r win7-x86
rm -Recurse -Force .\Publish
mkdir .\Publish
mkdir .\Publish\BleemSync
cp .\BleemSync\bin\Release\netcoreapp2.1\win7-x86\publish\* .\Publish\BleemSync
cp -Recurse .\BleemSync.Payload\* .\Publish
Compress-Archive -Force -Path .\Publish\* -CompressionLevel Optimal -DestinationPath .\BleemSync-$version-win7-x86.zip