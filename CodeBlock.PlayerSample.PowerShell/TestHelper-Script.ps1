# CodeBlock Test Helper Script
# Helps with ngrok setup and testing

param(
    [Parameter(Position=0)]
    [ValidateSet("start", "signup", "test-ready", "test-game", "monitor")]
    [string]$Command = "start",
    
    [int]$Port = 7080,
    [string]$TeamName = "PowerShellWarriors",
    [string]$GameEngineUrl = "https://localhost:7186"
)

function Start-NgrokAndPlayer {
    Write-Host "Starting ngrok on port $Port..." -ForegroundColor Yellow
    
    # Start ngrok in a new window
    Start-Process -FilePath "ngrok" -ArgumentList "http $Port" -WindowStyle Normal
    
    Write-Host "Waiting for ngrok to start..." -ForegroundColor Yellow
    Start-Sleep -Seconds 3
    
    # Get ngrok URL
    try {
        $ngrokApi = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels" -ErrorAction Stop
        $publicUrl = $ngrokApi.tunnels[0].public_url
        
        if ($publicUrl -match "^http://") {
            $publicUrl = $publicUrl -replace "^http://", "https://"
        }
        
        Write-Host "Ngrok URL: $publicUrl" -ForegroundColor Green
        
        # Start the player with the ngrok URL
        Write-Host "Starting player server..." -ForegroundColor Yellow
        $playerScript = Join-Path $PSScriptRoot "CodeBlock-Player.ps1"
        
        & $playerScript -Port $Port -TeamName $TeamName -GameEngineUrl $GameEngineUrl
    }
    catch {
        Write-Host "Failed to get ngrok URL. Make sure ngrok is installed and accessible." -ForegroundColor Red
        Write-Host "Error: $_" -ForegroundColor Red
    }
}

function Send-ManualSignUp {
    $ngrokApi = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels" -ErrorAction Stop
    $publicUrl = $ngrokApi.tunnels[0].public_url
    
    if ($publicUrl -match "^http://") {
        $publicUrl = $publicUrl -replace "^http://", "https://"
    }
    
    $body = @{
        PlayersBaseUrl = $publicUrl
        PlayersTeamName = $TeamName
    } | ConvertTo-Json
    
    Write-Host "Signing up with:" -ForegroundColor Cyan
    Write-Host "  URL: $publicUrl" -ForegroundColor Gray
    Write-Host "  Team: $TeamName" -ForegroundColor Gray
    
    try {
        $response = Invoke-RestMethod -Uri "$GameEngineUrl/api/signup" `
                                     -Method Post `
                                     -Body $body `
                                     -ContentType "application/json"
        Write-Host "Successfully signed up!" -ForegroundColor Green
    }
    catch {
        Write-Host "Failed to sign up: $_" -ForegroundColor Red
    }
}

function Test-ReadyEndpoint {
    Write-Host "Testing Ready endpoint..." -ForegroundColor Yellow
    try {
        $response = Invoke-RestMethod -Uri "http://localhost:$Port/api/Ready" -Method Get
        Write-Host "Ready endpoint working!" -ForegroundColor Green
    }
    catch {
        Write-Host "Ready endpoint failed: $_" -ForegroundColor Red
    }
}

function Test-GameFlow {
    Write-Host "Testing game flow..." -ForegroundColor Yellow
    
    # Test game initialisation
    $gameInit = @{
        TheBaseUrl = "https://test-game-engine.com"
        YourSecretKey = [Guid]::NewGuid()
        YourType = 0  # Red
    } | ConvertTo-Json
    
    try {
        Invoke-RestMethod -Uri "http://localhost:$Port/api/GameInitialisation" `
                         -Method Post `
                         -Body $gameInit `
                         -ContentType "application/json"
        Write-Host "Game initialisation: OK" -ForegroundColor Green
    }
    catch {
        Write-Host "Game initialisation failed: $_" -ForegroundColor Red
    }
    
    # Test bot information exchange
    try {
        $botInfo = Invoke-RestMethod -Uri "http://localhost:$Port/api/PlayerBotInformationExchange" -Method Get
        Write-Host "Bot information exchange: OK" -ForegroundColor Green
        Write-Host "Bots:" -ForegroundColor Gray
        Write-Host "  Bot 1: $($botInfo.FirstPlayerBot.BotId) (Painter)" -ForegroundColor Gray
        Write-Host "  Bot 2: $($botInfo.SecondPlayerBot.BotId) (Defense)" -ForegroundColor Gray
        Write-Host "  Bot 3: $($botInfo.ThirdPlayerBot.BotId) (Attack)" -ForegroundColor Gray
    }
    catch {
        Write-Host "Bot information exchange failed: $_" -ForegroundColor Red
    }
}

function Start-LogMonitor {
    Write-Host "Monitoring ngrok requests at http://localhost:4040" -ForegroundColor Cyan
    Write-Host "Press Ctrl+C to stop monitoring..." -ForegroundColor Yellow
    
    while ($true) {
        Start-Sleep -Seconds 5
        Clear-Host
        
        try {
            $requests = Invoke-RestMethod -Uri "http://localhost:4040/api/requests/http" -ErrorAction Stop
            
            Write-Host "=== Recent Requests ===" -ForegroundColor Cyan
            $requests.requests | Select-Object -First 10 | ForEach-Object {
                $timestamp = [DateTime]::Parse($_.start).ToString("HH:mm:ss")
                $method = $_.request.method
                $path = $_.request.path
                $status = $_.response.status
                
                $color = if ($status -ge 200 -and $status -lt 300) { "Green" } 
                         elseif ($status -ge 400) { "Red" } 
                         else { "Yellow" }
                
                Write-Host "[$timestamp] $method $path - $status" -ForegroundColor $color
            }
        }
        catch {
            Write-Host "No requests yet or ngrok not running" -ForegroundColor Gray
        }
    }
}

# Main execution
Write-Host @"
===================================
CodeBlock Test Helper
===================================
Command: $Command
Port: $Port
Team: $TeamName
===================================
"@ -ForegroundColor Cyan

switch ($Command) {
    "start" {
        Start-NgrokAndPlayer
    }
    "signup" {
        Send-ManualSignUp
    }
    "test-ready" {
        Test-ReadyEndpoint
    }
    "test-game" {
        Test-GameFlow
    }
    "monitor" {
        Start-LogMonitor
    }
}