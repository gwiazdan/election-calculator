# Uruchamianie backendu jako job w tle
Write-Host "Uruchamianie backendu..."
$backendDir = "D:\Projekty\Web\election-calculator\backend"
Push-Location -Path $backendDir
Start-Job -Name "BackendDevServer" -ScriptBlock {
    dotnet run
}
Pop-Location

# Uruchamianie frontendu jako job w tle
Write-Host "Uruchamianie frontendu..."
$frontendDir = "D:\Projekty\Web\election-calculator\frontend"
Push-Location -Path $frontendDir
Start-Job -Name "FrontendDevServer" -ScriptBlock {
    npm run dev
}
Pop-Location

Write-Host "Backend i frontend uruchomione jako zadania w tle."
Write-Host "Naciśnij Enter, aby zakończyć pracę i zatrzymać serwery."
Read-Host

Write-Host "Zatrzymywanie zadań i procesów..."

# Zatrzymujemy joby
Get-Job | Where-Object { $_.Name -in @("BackendDevServer", "FrontendDevServer") } | ForEach-Object {
    if ($_.State -eq "Running") {
        Stop-Job -Job $_
        # Poczekaj na zatrzymanie zadania
        while ($_.State -eq "Running") {
            Start-Sleep -Milliseconds 200
            $_ = Get-Job -Id $_.Id
        }
    }
    Remove-Job -Job $_
}
