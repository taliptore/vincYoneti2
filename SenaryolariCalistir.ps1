# TORE VINC - Senaryolari calistir: API + Admin Vue
# Kullanim: .\SenaryolariCalistir.ps1
# Tarayicida: http://localhost:5173 veya 5174 (Vite hangi portu acmissa)

$apiPath = Join-Path $PSScriptRoot "src\CraneManagementSystem.API"
$vuePath = Join-Path $PSScriptRoot "admin-vue"

Write-Host "=== API kontrol ===" -ForegroundColor Cyan
$apiOk = $false
try {
    $r = Invoke-WebRequest -Uri "http://localhost:5116/swagger/index.html" -UseBasicParsing -TimeoutSec 2 -ErrorAction Stop
    $apiOk = ($r.StatusCode -eq 200)
} catch {}
if ($apiOk) {
    Write-Host "API zaten calisiyor: http://localhost:5116" -ForegroundColor Green
} else {
    Write-Host "API baslatiliyor..." -ForegroundColor Yellow
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$apiPath'; dotnet run"
    Start-Sleep -Seconds 8
}

Write-Host "`n=== Admin Vue kontrol ===" -ForegroundColor Cyan
$vueOk = $false
foreach ($port in 5173, 5174) {
    try {
        $r = Invoke-WebRequest -Uri "http://localhost:$port" -UseBasicParsing -TimeoutSec 2 -ErrorAction Stop
        $vueOk = ($r.StatusCode -eq 200)
        if ($vueOk) {
            Write-Host "Vue zaten calisiyor: http://localhost:$port" -ForegroundColor Green
            $vuePort = $port
            break
        }
    } catch {}
}
if (-not $vueOk) {
    Write-Host "Vue baslatiliyor..." -ForegroundColor Yellow
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$vuePath'; npm run dev"
    Start-Sleep -Seconds 5
    $vuePort = 5173
}

Write-Host "`n=== Panel adresi ===" -ForegroundColor Cyan
if ($vuePort) { Write-Host "http://localhost:$vuePort" -ForegroundColor White }
else { Write-Host "http://localhost:5173 veya 5174" -ForegroundColor White }
Write-Host "`nGiris: appsettings.json Seed:Admin email/sifre ile giris yapin." -ForegroundColor Gray
