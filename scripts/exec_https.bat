
@echo off

set SCRIPT_DIR=%~dp0

cd /d "%SCRIPT_DIR%\.."

echo Cleaning the project... Standby
dotnet clean
if %errorlevel% neq 0 (
    echo Error during dotnet clean. Exiting.
    exit /b %errorlevel%
)

echo Building the project... Standby
dotnet build
if %errorlevel% neq 0 (
    echo Error during dotnet build. Exiting.
    exit /b %errorlevel%
)

echo Running the project with the HTTPS launch profile...
dotnet run --launch-profile https
if %errorlevel% neq 0 (
    echo Error during dotnet run. Exiting.
    exit /b %errorlevel%
)

echo Script completed successfully.
pause

