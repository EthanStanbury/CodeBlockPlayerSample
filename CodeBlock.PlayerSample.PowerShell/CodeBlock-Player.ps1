# CodeBlock Red vs Blue - PowerShell Player Implementation
# Run this script to start your player server

param(
    [int]$Port = 7080,
    [string]$TeamName = "PowerShellWarriors",
    [string]$GameEngineUrl = "https://localhost:7186",
    [switch]$AutoSignUp
)

# Ignore SSL certificate errors for localhost development
if ($PSVersionTable.PSVersion.Major -lt 6) {
    Add-Type @"
        using System.Net;
        using System.Security.Cryptography.X509Certificates;
        public class TrustAllCertsPolicy : ICertificatePolicy {
            public bool CheckValidationResult(
                ServicePoint srvPoint, X509Certificate certificate,
                WebRequest request, int certificateProblem) {
                return true;
            }
        }
"@
    [System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
}

# Global game state
$Global:GameState = @{
    GameEndpointBaseUrl = ""
    Bot1Id              = [Guid]::NewGuid()
    Bot2Id              = [Guid]::NewGuid()
    Bot3Id              = [Guid]::NewGuid()
    EndpointSecret      = [Guid]::Empty
    PlayerType          = "Neutral"
    TeamName            = $TeamName
    BaseUrl             = "http://localhost:$Port"
}

Write-Host "Initialised bot IDs:" -ForegroundColor Gray
Write-Host "  Bot 1 (Painter): $($Global:GameState.Bot1Id)" -ForegroundColor Gray
Write-Host "  Bot 2 (Defense): $($Global:GameState.Bot2Id)" -ForegroundColor Gray
Write-Host "  Bot 3 (Attack): $($Global:GameState.Bot3Id)" -ForegroundColor Gray

# Enums
$Global:PlayerType = @{
    Red     = 0
    Blue    = 1
    Neutral = 2
}

$Global:BotType = @{
    Painter = 0
    Attack  = 1
    Defense = 2
    Empty   = 3
}

$Global:MoveAction = @{
    Strike = 0
    Defend = 1
    Paint  = 2
    Wait   = 3
}

#region Helper Functions

function New-Position {
    param(
        [int]$X,
        [int]$Y
    )
    return @{
        x = $X
        y = $Y
    }
}

function New-Move {
    param(
        [hashtable]$Position,
        [string]$Action,
        [hashtable]$TargetPosition = $null
    )
    $move = @{
        action   = $Action
        position = $Position
    }
    if ($TargetPosition) {
        $move.targetPosition = $TargetPosition
    }
    return $move
}

function Find-Bot {
    param(
        [object]$Board,
        [Guid]$BotId
    )
    
    foreach ($column in $Board.boardGrid) {
        foreach ($square in $column) {
            if ($square.onSquare -and $square.onSquare.botId -eq $BotId) {
                return $square
            }
        }
    }
    return $null
}

#endregion

#region Movement Functions

# Painting movements
function Move-BotNorthAndPaint($Position) {
    return New-Move -Position (New-Position -X $Position.x -Y ($Position.y + 1)) -Action $Global:MoveAction.Paint
}

function Move-BotSouthAndPaint($Position) {
    return New-Move -Position (New-Position -X $Position.x -Y ($Position.y - 1)) -Action $Global:MoveAction.Paint
}

function Move-BotEastAndPaint($Position) {
    return New-Move -Position (New-Position -X ($Position.x + 1) -Y $Position.y) -Action $Global:MoveAction.Paint
}

function Move-BotWestAndPaint($Position) {
    return New-Move -Position (New-Position -X ($Position.x - 1) -Y $Position.y) -Action $Global:MoveAction.Paint
}

function Move-BotSoutheastAndPaint($Position) {
    return New-Move -Position (New-Position -X ($Position.x + 1) -Y ($Position.y - 1)) -Action $Global:MoveAction.Paint
}

# Defending movements
function Move-BotNorthAndDefend($Position) {
    return New-Move -Position (New-Position -X $Position.x -Y ($Position.y + 1)) -Action $Global:MoveAction.Defend
}

function Move-BotSouthAndDefend($Position) {
    return New-Move -Position (New-Position -X $Position.x -Y ($Position.y - 1)) -Action $Global:MoveAction.Defend
}

function Move-BotEastAndDefend($Position) {
    return New-Move -Position (New-Position -X ($Position.x + 1) -Y $Position.y) -Action $Global:MoveAction.Defend
}

function Move-BotWestAndDefend($Position) {
    return New-Move -Position (New-Position -X ($Position.x - 1) -Y $Position.y) -Action $Global:MoveAction.Defend
}

# Attacking movements
function Move-BotSoutheastAndStrikeEast($Position) {
    return New-Move -Position (New-Position -X ($Position.x + 1) -Y ($Position.y - 1)) `
        -Action $Global:MoveAction.Strike `
        -TargetPosition (New-Position -X ($Position.x + 2) -Y ($Position.y - 1))
}

function Move-BotSoutheastAndStrikeWest($Position) {
    return New-Move -Position (New-Position -X ($Position.x + 1) -Y ($Position.y - 1)) `
        -Action $Global:MoveAction.Strike `
        -TargetPosition (New-Position -X $Position.x -Y ($Position.y - 1))
}

function Move-BotSoutheastAndStrikeSouth($Position) {
    return New-Move -Position (New-Position -X ($Position.x + 1) -Y ($Position.y - 1)) `
        -Action $Global:MoveAction.Strike `
        -TargetPosition (New-Position -X ($Position.x + 1) -Y ($Position.y - 2))
}

#endregion

#region Bot Strategy Functions

function Get-FirstBotMoves {
    param([object]$Board)
    
    $botSquare = Find-Bot -Board $Board -BotId $Global:GameState.Bot1Id
    if (-not $botSquare) {
        throw "Couldn't find first bot"
    }
    
    $position = $botSquare.XYPosition
    $moves = @()
    
    # Bot moves along top row then down right side
    for ($i = 0; $i -lt 3; $i++) {
        if ($position.X -eq 9) {
            $moves += Move-BotSouthAndPaint $position
            $position = @{ X = $position.X; Y = $position.Y - 1 }
        }
        else {
            $moves += Move-BotEastAndPaint $position
            $position = @{ X = $position.X + 1; Y = $position.Y }
        }
    }
    
    return @{
        playerBot = $botSquare.OnSquare
        moves     = $moves
    }
}

function Get-SecondBotMoves {
    param([object]$Board)
    
    $botSquare = Find-Bot -Board $Board -BotId $Global:GameState.Bot2Id
    if (-not $botSquare) {
        throw "Couldn't find second bot"
    }
    
    $position = $botSquare.XYPosition
    
    return @{
        playerBot = $botSquare.OnSquare
        moves     = @(
            Move-BotSoutheastAndStrikeEast $position
            Move-BotSoutheastAndStrikeWest $position
            Move-BotSoutheastAndStrikeSouth $position
        )
    }
}

function Get-ThirdBotMoves {
    param([object]$Board)
    
    $botSquare = Find-Bot -Board $Board -BotId $Global:GameState.Bot3Id
    if (-not $botSquare) {
        throw "Couldn't find third bot"
    }
    
    $position = $botSquare.XYPosition
    $moves = @()
    
    # Bot moves down left column then across bottom row
    for ($i = 0; $i -lt 3; $i++) {
        if ($position.Y -eq 9) {
            $moves += Move-BotEastAndDefend $position
            $position = @{ X = $position.X + 1; Y = $position.Y }
        }
        else {
            $moves += Move-BotSouthAndDefend $position
            $position = @{ X = $position.X; Y = $position.Y - 1 }
        }
    }
    
    return @{
        playerBot = $botSquare.OnSquare
        moves     = $moves
    }
}

#endregion

#region API Functions

function Send-SignUp {
    $body = @{
        playersBaseUrl  = $Global:GameState.BaseUrl
        playersTeamName = $Global:GameState.TeamName
    } | ConvertTo-Json
    
    try {
        $params = @{
            Uri         = "$GameEngineUrl/api/signup"
            Method      = "Post"
            Body        = $body
            ContentType = "application/json"
        }
        
        if ($PSVersionTable.PSVersion.Major -ge 6) {
            $params.SkipCertificateCheck = $true
        }
        
        $response = Invoke-RestMethod @params
        Write-Host "Successfully signed up team: $($Global:GameState.TeamName)" -ForegroundColor Green
    }
    catch {
        Write-Host "Failed to sign up: $_" -ForegroundColor Red
    }
}

function Send-TurnInput {
    param([object]$TurnInput)
    
    $endpoint = "$($Global:GameState.GameEndpointBaseUrl)/api/$($Global:GameState.PlayerType)Input"
    $credentials = "$($Global:GameState.PlayerType):$($Global:GameState.EndpointSecret)"
    $encodedCreds = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($credentials))
    
    $headers = @{
        Authorization = "Basic $encodedCreds"
    }
    
    try {
        $params = @{
            Uri         = $endpoint
            Method      = "Post"
            Headers     = $headers
            Body        = ($TurnInput | ConvertTo-Json -Depth 10)
            ContentType = "application/json"
        }
        
        if ($PSVersionTable.PSVersion.Major -ge 6) {
            $params.SkipCertificateCheck = $true
        }
        
        $response = Invoke-RestMethod @params
        Write-Host "Turn submitted successfully" -ForegroundColor Green
    }
    catch {
        Write-Host "Failed to submit turn: $_" -ForegroundColor Red
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $responseBody = $reader.ReadToEnd()
            Write-Host "Response: $responseBody" -ForegroundColor Yellow
        }
    }
}

#endregion

#region HTTP Server

function Start-PlayerServer {
    $listener = New-Object System.Net.HttpListener
    $listener.Prefixes.Add("http://+:$Port/")
    
    try {
        $listener.Start()
        Write-Host "Player server started on port $Port" -ForegroundColor Green
        Write-Host "Base URL: $($Global:GameState.BaseUrl)" -ForegroundColor Cyan
        Write-Host "Press Ctrl+C to stop..." -ForegroundColor Yellow
        
        while ($listener.IsListening) {
            $context = $listener.GetContext()
            $request = $context.Request
            $response = $context.Response
            
            Write-Host "[$([DateTime]::Now.ToString('HH:mm:ss'))] $($request.HttpMethod) $($request.Url.LocalPath)" -ForegroundColor Gray
            
            try {
                switch -Regex ($request.Url.LocalPath) {
                    "^/api/Ready$" {
                        if ($request.HttpMethod -eq "GET") {
                            $response.StatusCode = 200
                            Write-Host "Ready check: OK" -ForegroundColor Green
                        }
                    }
                    
                    "^/api/GameInitialisation$" {
                        if ($request.HttpMethod -eq "POST") {
                            $reader = New-Object System.IO.StreamReader($request.InputStream)
                            $bodyText = $reader.ReadToEnd()
                            Write-Host "Game initialisation payload:" -ForegroundColor Gray
                            Write-Host $bodyText -ForegroundColor Gray
                            
                            $body = $bodyText | ConvertFrom-Json
                            
                            $Global:GameState.GameEndpointBaseUrl = $body.TheBaseUrl
                            $Global:GameState.EndpointSecret = $body.YourSecretKey
                            $Global:GameState.PlayerType = switch ($body.YourType) {
                                0 { "Red" }
                                1 { "Blue" }
                                default { "Neutral" }
                            }
                            
                            Write-Host "Game initialised - Type: $($Global:GameState.PlayerType), Secret: $($Global:GameState.EndpointSecret)" -ForegroundColor Green
                            $response.StatusCode = 200
                        }
                    }
                    
                    "^/api/GameInitialisationHandshake$" {
                        if ($request.HttpMethod -eq "GET") {
                            
                            $botInfo = @{
                                FirstPlayerBot  = @{
                                    BotId = $Global:GameState.Bot1Id
                                    Type  = 0  # Painter
                                }
                                SecondPlayerBot = @{
                                    BotId = $Global:GameState.Bot2Id
                                    Type  = 2  # Defense
                                }
                                ThirdPlayerBot  = @{
                                    BotId = $Global:GameState.Bot3Id
                                    Type  = 1  # Attack
                                }
                            } | ConvertTo-Json
                            
                            Write-Host "Returning bot info:" -ForegroundColor Gray
                            Write-Host $botInfo -ForegroundColor Gray
                            
                            $buffer = [Text.Encoding]::UTF8.GetBytes($botInfo)
                            $response.ContentType = "application/json"
                            $response.OutputStream.Write($buffer, 0, $buffer.Length)
                            Write-Host "Game initialisation handshake completed" -ForegroundColor Green
                        }
                    }
                    
                    "^/api/BoardUpdate$" {
                        if ($request.HttpMethod -eq "POST") {
                            $reader = New-Object System.IO.StreamReader($request.InputStream)
                            $bodyText = $reader.ReadToEnd()
                            
                            Write-Host "Received board update (first 500 chars):" -ForegroundColor Gray
                            Write-Host $bodyText.Substring(0, [Math]::Min(500, $bodyText.Length)) -ForegroundColor Gray
                            
                            $body = $bodyText | ConvertFrom-Json
                            
                            $board = $body.currentBoardState
                            
                            # Generate moves for each bot
                            $turnInput = @{
                                timeStamp    = $body.NewTurnTimestamp
                                turnMoveSets = @(
                                    Get-FirstBotMoves -Board $board
                                    Get-SecondBotMoves -Board $board
                                    Get-ThirdBotMoves -Board $board
                                )
                            }
                            
                            # Submit turn to game engine
                            Send-TurnInput -TurnInput $turnInput
                            
                            $response.StatusCode = 202
                            Write-Host "Board update processed and turn submitted" -ForegroundColor Green
                        }
                    }
                    
                    "^/api/GameCompletion$" {
                        if ($request.HttpMethod -eq "POST") {
                            Write-Host "Game completed!" -ForegroundColor Cyan
                            $response.StatusCode = 200
                        }
                    }
                    
                    default {
                        $response.StatusCode = 404
                        Write-Host "Unknown endpoint: $($request.Url.LocalPath)" -ForegroundColor Yellow
                    }
                }
            }
            catch {
                Write-Host "Error processing request: $_" -ForegroundColor Red
                $response.StatusCode = 500
            }
            finally {
                $response.Close()
            }
        }
    }
    finally {
        $listener.Stop()
        Write-Host "Server stopped" -ForegroundColor Red
    }
}

#endregion

# Main execution
Write-Host @"
===================================
CodeBlock: Red vs Blue - PowerShell Player
===================================
Team Name: $($Global:GameState.TeamName)
Port: $Port
Game Engine: $GameEngineUrl
===================================
"@ -ForegroundColor Cyan

if ($AutoSignUp) {
    Write-Host "Auto-signing up with game engine..." -ForegroundColor Yellow
    Send-SignUp
}
else {
    Write-Host "To sign up manually, run: Send-SignUp" -ForegroundColor Yellow
}

# Start the server
Start-PlayerServer