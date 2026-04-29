
# powershell -ExecutionPolicy Bypass -File .\update-db.ps1

$time = Get-Date -Format "HHmm"
$date = Get-Date -Format "yyyyMMdd"
# Caminho das migrations agora sobe dois níveis para achar a pasta certa
$rootMigrations = "../../src/Health.Infrastructure/Adapters/Postgres/Migrations"
$targetPath = "$rootMigrations/$date"

# Carrega as variáveis do arquivo .env para a sessão atual do PowerShell
Get-Content .env | Foreach-Object {
    if ($_ -match "^([^=]+)=(.*)$") {
        $name = $matches[1].Trim()
        $value = $matches[2].Trim()
        [System.Environment]::SetEnvironmentVariable($name, $value, "Process")
    }
}

Write-Host "Variáveis de ambiente carregadas do .env" -ForegroundColor Magenta

Write-Host "--- Iniciando Processo de Migrations (Twelve-Factor Mode) ---" -ForegroundColor Cyan

# Cria a pasta do dia se não existir
if (!(Test-Path $targetPath)) { New-Item -ItemType Directory -Path $targetPath }

$migrations = @("Plano_$time", "Beneficiario_$time", "ExclusaoBeneficiarioFila_$time")

foreach ($name in $migrations) {
    Write-Host "Gerando: $name" -ForegroundColor Blue
    
    # dotnet ef comandos: Subindo dois níveis para achar os projetos
    dotnet ef migrations add $name `
        -p ../../src/Health.Infrastructure `
        -s ../../src/Health.API `
        -o "Adapters/Postgres/Migrations"

    if ($LASTEXITCODE -eq 0) {
        # Move os arquivos gerados para a pasta do dia
        Get-ChildItem -Path $rootMigrations -Filter "*_$name.cs" | Move-Item -Destination $targetPath
        Write-Host "Migration movida para $targetPath" -ForegroundColor Gray
    } else {
        Write-Host "Erro ao gerar $name" -ForegroundColor Red
        exit
    }
}

Write-Host "Sincronizando com o Postgres local..." -ForegroundColor Yellow
# Update também precisa subir dois níveis
dotnet ef database update -p ../../src/Health.Infrastructure -s ../../src/Health.API

Write-Host "--- PROCESSO CONCLUIDO ---" -ForegroundColor Green