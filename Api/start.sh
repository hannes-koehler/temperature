# dotnet run /dbname=../data/measurement.sqlite
dotnet publish -c Release -r linux-arm --self-contained
./bin/Release/net8.0/linux-arm/testapi /dbname=../data/measurement.sqlite