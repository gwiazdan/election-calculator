# Skrypt do uruchamiania środowiska deweloperskiego (frontend i backend)

Write-Host "Uruchamianie serwera deweloperskiego dla backendu jako zadanie w tle..."
$backendDir = "D:\Projekty\Web\election-calculator\backend"
Push-Location -Path $backendDir
Start-Job -Name "BackendDevServer" -ScriptBlock {
    Write-Host "Backend (job): Przechodzenie do $(Get-Location). Uruchamianie dotnet run..."
    dotnet run
}
Pop-Location
Write-Host "Serwer deweloperski backendu uruchomiony jako zadanie w tle."
Write-Host "Możesz sprawdzić jego status za pomocą polecenia 'Get-Job BackendDevServer'."
Write-Host "Aby zobaczyć jego wyjście, użyj polecenia 'Receive-Job BackendDevServer'."
Write-Host ""

Write-Host "Uruchamianie serwera deweloperskiego dla frontendu w bieżącym oknie..."
$frontendDir = "D:\Projekty\Web\election-calculator\frontend"
Push-Location -Path $frontendDir
Write-Host "Frontend: Przechodzenie do $(Get-Location). Uruchamianie npm run dev..."
Write-Host "To polecenie będzie działać w pierwszym planie. Naciśnij Ctrl+C, aby je zatrzymać."
# Skrypt zatrzyma się tutaj, aż npm run dev zostanie zatrzymane
npm run dev
Pop-Location

Write-Host "Serwer deweloperski frontendu zatrzymany."
Write-Host "Skrypt zakończony. Naciśnij Enter, aby wyjść."
Read-Host