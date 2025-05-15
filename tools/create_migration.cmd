@echo off
setlocal enabledelayedexpansion

REM ================================
REM Cria uma nova migration e script SQL
REM ================================

REM Nome da migration passado como argumento
set MIGRATION_NAME=%1

IF "%MIGRATION_NAME%"=="" (
    echo [ERRO] Nome da migration é obrigatório.
    echo Uso: .\create_migration.cmd NomeDaMigration
    exit /b 1
)

REM Caminhos relativos à pasta /tools
set MIGRATION_DIR=../ApiProject.Infrastructure/Migrations
set SQL_DIR=../migrations

REM Verifica se já existe uma migration com este nome
IF EXIST "%MIGRATION_DIR%\%MIGRATION_NAME%*.cs" (
    echo [ERRO] Já existe uma migration chamada "%MIGRATION_NAME%".
    exit /b 1
)

REM Cria nova migration
dotnet ef migrations add %MIGRATION_NAME% ^
    --project "../src/ApiProject.Infrastructure" ^
    --startup-project "../src/ApiProject" ^
    --output-dir Migrations ^
    --context ApiProjectDbContext

IF %ERRORLEVEL% NEQ 0 (
    echo [ERRO] Falha ao criar a migration.
    exit /b 1
)

echo [OK] Migration criada: %MIGRATION_NAME%

REM ================================
REM Identifica migrations válidas (exclui Snapshot e Designer)
REM ================================

set MIG_NEW=
set MIG_OLD=
set COUNT=0

REM Caminho absoluto da pasta de migrations
pushd "%~dp0..\src\ApiProject.Infrastructure\Migrations" || (
    echo [ERRO] Falha ao acessar pasta de migrations.
    exit /b 1
)

echo [DEBUG] Listando arquivos em: %CD%
dir /b

REM Loop para encontrar as duas migrations big_bang mais recentes
for /f "delims=" %%A in ('dir /b /o-d *big_bang*.cs 2^>nul') do (
    echo %%A | find /i "Snapshot" >nul
    if ERRORLEVEL 1 (
        echo %%A | find /i "Designer" >nul
        if ERRORLEVEL 1 (
            set /a COUNT+=1
            if not defined MIG_NEW (
                set "MIG_NEW=%%~nA"
            ) else if not defined MIG_OLD (
                set "MIG_OLD=%%~nA"
                goto voltar
            )
        )
    )
)
)

:voltar
popd

IF "!MIG_NEW!"=="" (
    echo [ERRO] Nenhuma migration válida foi encontrada.
    exit /b 1
)

REM ================================
REM Gera o script SQL
REM ================================

IF "!COUNT!"=="1" (
    echo [INFO] Apenas uma migration encontrada. Gerando script completo.

    dotnet ef migrations script ^
        --project ../src/ApiProject.Infrastructure ^
        --startup-project ../src/ApiProject ^
        --context ApiProjectDbContext ^
        -o %SQL_DIR%\%MIGRATION_NAME%.sql
) ELSE (
    echo [INFO] Gerando script de !MIG_OLD! até !MIG_NEW!.

    dotnet ef migrations script !MIG_OLD! !MIG_NEW! ^
        --project ../src/ApiProject.Infrastructure ^
        --startup-project ../src/ApiProject ^
        --context ApiProjectDbContext ^
        -o %SQL_DIR%\%MIGRATION_NAME%.sql
)

IF %ERRORLEVEL% NEQ 0 (
    echo [ERRO] Falha ao gerar o script SQL.
    exit /b 1
)

echo [OK] Script gerado em: %SQL_DIR%\%MIGRATION_NAME%.sql
exit /b 0
