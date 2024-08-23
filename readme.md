# Readme

## Links

- [Blazor Fluent UI](https://github.com/microsoft/fluentui-blazor)
- [Demo site](https://www.fluentui-blazor.net/)
- [APEX Charts](https://github.com/apexcharts/Blazor-ApexCharts)

## Setup

- https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- dotnet new install Microsoft.FluentUI.AspNetCore.Templates
- dotnet new fluentblazorwasm --name MyApplication

## Raspi
- wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet8pi/main/install.sh | sudo bash
dotnet publish -c Release -r linux-arm --self-contained