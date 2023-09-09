@ECHO OFF

IF /I "%~1"=="/?" GOTO help
IF /I "%~1"=="--help" GOTO help

IF /I "%~1"=="clean" GOTO clean
IF /I "%~1"=="test" GOTO test
IF /I "%~1"=="app" GOTO app
IF /I "%~1"=="coverage" GOTO coverage
IF /I "%~1"=="mutation" GOTO mutation

:help
ECHO.
ECHO Runs the application.
ECHO.
ECHO Usage:
ECHO %~0 [clean] [test] [run] [coverage] [mutation]
ECHO.
ECHO Parameter List:
ECHO     clean      Clears the node_modules directory and other generated files.
ECHO.
ECHO     test       Test the project.
ECHO.
ECHO     app        Runs the application.
ECHO.
ECHO     coverage   Test the project and generate a coverage report.
ECHO.
ECHO     mutation   Run mutation testing.
ECHO.
ECHO.    /?         Displays this help message.

GOTO end

:clean
ECHO.
ECHO Removing binary files...
CALL dotnet clean
for /f %%i in ('dir bin /s /b') do rd /s /q %%i
for /f %%i in ('dir obj /s /b') do rd /s /q %%i
for /f %%i in ('dir StrykerOutput /s /b') do rd /s /q %%i

IF /I "%~2"=="test" GOTO test
IF /I "%~2"=="coverage" GOTO coverage
IF /I "%~2"=="mutation" GOTO mutation

GOTO end

:test
ECHO.
ECHO Starting the integration test database...
CALL docker-compose -f IntegrationTest.Infra.MongoDB/docker-compose.yml up -d

ECHO.
ECHO Resolving dependencies...
CALL dotnet restore --force --no-cache

ECHO.
ECHO Building the project...
CALL dotnet build --configuration Debug --no-restore

ECHO.
ECHO Running test...
CALL dotnet test --no-build --nologo -v q

GOTO end

:app
ECHO.
ECHO Resolving dependencies...
CALL dotnet restore --force --no-cache

ECHO.
ECHO Building the project...
CALL dotnet build --configuration Debug --no-restore

ECHO.
ECHO Start browsing...
START http://localhost:5132/api/properties-search/v1/swagger

ECHO.
ECHO Running the server...
CALL dotnet run --no-build --no-restore --nologo --project ./EntryPoint.WebApi/EntryPoint.WebApi.csproj

GOTO end

:coverage
ECHO.
ECHO Starting the integration test database...
CALL docker-compose -f IntegrationTest.Infra.MongoDB/docker-compose.yml up -d

ECHO.
ECHO Resolving dependencies...
CALL dotnet restore --force --no-cache

ECHO.
ECHO Building the project...
CALL dotnet build --configuration Debug --no-restore

ECHO.
ECHO Running test...
dotnet test --no-build --nologo -v q

ECHO.
ECHO Opening code coverage...
:: CALL START coverage\lcov-report\index.html

GOTO end

:mutation
ECHO.
ECHO Starting the integration test database...
CALL docker-compose -f IntegrationTest.Infra.MongoDB/docker-compose.yml up -d

ECHO.
ECHO Resolving dependencies...
CALL dotnet restore --force --no-cache

ECHO.
ECHO Building the project...
CALL dotnet build --configuration Debug --no-restore

ECHO.
ECHO Running Mutation test...
dotnet stryker -o

GOTO end

:end
