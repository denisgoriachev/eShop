echo off

if "%~1"=="" (
    echo "Migration name is empty"; 
    exit /b 1;
)

dotnet ef migrations add "%~1" -p "..\..\..\eShop.Infrastructure" -s "..\..\..\eShop.Api" -o "Persistance\Migrations"