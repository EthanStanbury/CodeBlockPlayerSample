# CodeBlock: Red vs Blue

Welcome to CodeBlock: Red vs Blue, a strategic turn-based game where two teams compete for control of the game board using specialized bots.

## Game Overview

CodeBlock: Red vs Blue is a turn-based tactical game played on a grid. Each player controls a team (Red or Blue) of three specialized bots. Players take turns moving their bots across the board, performing actions based on their bot types, and attempting to control territory.

The game is played entirely through REST API calls, allowing you to build your own client or automation to interact with the game server.

## Bot Types

Each team has three bots with different capabilities:

### 1. Painter Bot

- **Primary Action**: Paint
- **Purpose**: Claims territory by painting squares your team's color
- **Strategy**: Use Painter bots to expand your territory and control the board
- **Movement**: Can move up to 3 squares per turn

### 2. Attack Bot

- **Primary Action**: Strike
- **Purpose**: Temporarily incapacitates enemy bots
- **Effect**: When an Attack bot successfully strikes an enemy bot, the enemy becomes incapacitated for a set number of turns
- **Strategy**: Target enemy Painter bots to prevent territory expansion or Defense bots to open up vulnerabilities
- **Movement**: Can move up to 3 squares per turn
- **Special**: Must specify a target position adjacent to its position to attack

### 3. Defense Bot

- **Primary Action**: Defend
- **Purpose**: Protects territory from being painted by enemy Painter bots
- **Effect**: Squares defended by a Defense bot cannot be repainted by enemy Painter bots or have a bot attacked within it
- **Strategy**: Defend critical squares or protect valuable territory
- **Movement**: Can move up to 2 squares per turn

## Game Mechanics

### Board

- The game board is a grid with dimensions of 100x100 (0-99 on both x and y axes)
- Each square can be neutral or controlled by either Red or Blue team
- Bots occupy squares on the board and can move to adjacent squares each turn

#### Example Board (10x10 Scale)

Below is a simplified representation of a 10x10 game board in progress:

```
   0  1  2  3  4  5  6  7  8  9  (X)
0  N  N  N  N  N  N  N  N  BP N
1  N  N  B  B  B  N  N  N  B  N
2  N  N  B  B  BD B  N  N  N  N
3  N  N  B  B  B  N  N  N  N  N
4  N  BA N  N  N  N  N  N  N  N
5  N  N  N  N  N  N  N  N  N  N
6  N  N  N  N  N  R  R  R  N  N
7  N  RA N  N  N  R  RP R  N  N
8  N  N  N  N  N  R  R  R  N  N
9  N  N  N  N  N  RD N  N  N  N
(Y)
```

Legend:

- `N`: Neutral square
- `R`: Red team's controlled square
- `B`: Blue team's controlled square
- `RP`/`BP`: Red/Blue Painter Bot
- `RA`/`BA`: Red/Blue Attack Bot
- `RD`/`BD`: Red/Blue Defense Bot

As the game progresses, more squares will change from neutral (N) to either Red (R) or Blue (B) as Painter bots claim territory. The actual game board is 100x100, allowing for much more complex strategies and territory control.

### Turns

1. Each player submits their turn through the API
2. Turns consist of moves for each of their bots
3. The order the moves are provided for the bot, are the order they are exectued
4. Moves specify the bot, its action, and position
5. After both players submit their turns, the game engine processes them and updates the game state
6. The updated board state is sent to the opposing player
7. Players then plan and submit their next turn

### Actions

- **Paint**: Changes a square to your team's colour
- **Strike**: Incapacitates an enemy bot for a set number of turns
- **Defend**: Protects a square from being painted by enemy bots or attacking your friendly bots
- **Wait**: The bot remains in place and performs no action

### Movement Rules

- Bots can only move to adjacent squares (horizontal, vertical, or diagonal)
- Bots cannot move more than one square at a time
- The number of moves a bot can make per turn depends on its type
- Moves must be within the board boundaries (0-99 on both axes)
- Bots cannot move through or occupy the same square as another bot

### Bot Status

- **Alive**: The bot is active and can perform actions
- **Incapacitated**: The bot cannot move or perform actions for a set number of turns
- **Collided**: The bot attempted to move to an occupied square
- **Unassigned**: The bot is not currently in play

## API Interaction

### Authentication

Most API calls to the game engine require Basic Authentication using your team's secret key:

```
Authorization: Basic {yourSecretKey}
```

**When to use authentication:**

- ✅ When submitting turns: `POST /api/RedInput` or `POST /api/BlueInput`
- ❌ When signing up: `POST /api/SignUp` (no auth required)
- ❌ For endpoints on YOUR server (the game engine calls you)

**Important**: You receive your secret key when the game engine calls your `/api/GameInitialisation` endpoint. Store it securely and use it for all subsequent calls to the game engine during that match.

### Player Registration (Sign Up)

Before participating in games, you must register your player with the game engine:

```
POST {gameEngineBaseUrl}/api/SignUp
```

Request body:

```json
{
  "playersBaseUrl": "https://your-player-url.com", // Your player's base URL (use ngrok URL for local development)
  "playersTeamName": "YourTeamName" // Your unique team name
}
```

Response:

- **201 Created**: Successfully registered
- **400 Bad Request**: Invalid request (e.g., missing fields, duplicate team name)

**Note**: The sign-up endpoint does not require authentication. You'll receive your secret key during the game initialization process.

**Important**: When developing locally, use your ngrok URL as the `playersBaseUrl`. For example:

```json
{
  "playersBaseUrl": "https://abc123.ngrok.io",
  "playersTeamName": "AwesomeBots"
}
```

Once deployed to Azure, update your registration with your Azure App Service URL:

```json
{
  "playersBaseUrl": "https://yourapp.azurewebsites.net",
  "playersTeamName": "AwesomeBots"
}
```

### Game Flow Overview

1. **Registration Phase**

   - You → Game Engine: `POST /api/SignUp` with your base URL and team name
   - Game Engine adds you to the player pool

2. **Matchmaking Phase** (handled by game engine)

   - Game Engine matches you with another registered player
   - Game Engine initiates the game setup

3. **Game Initialization Phase**

   - Game Engine → You: `GET /api/Ready` (checks if you're online)
   - You → Game Engine: Return 200 OK
   - Game Engine → You: `POST /api/GameInitialisation` (provides secret key and game details)
   - Game Engine → You: `GET /api/PlayerBotInformationExchange` (requests your bot IDs)
   - You → Game Engine: Return your three bot configurations

4. **Game Play Phase**

   - Game Engine → You: `POST /api/BoardUpdate` (sends current board state)
   - You → Game Engine: `POST /api/RedInput` or `/api/BlueInput` (submit your moves)
   - Repeat until game ends

5. **Game Completion Phase**
   - Game Engine → You: `POST /api/GameCompletion` (notifies game has ended)
   - Check leaderboard and match history for results

### Game Initialisation Flow

After registering, the game engine will match you with another player and initiate games with your player. **You don't need to take any action** - the game engine will call your endpoints when a match is ready.

1. **Ready Check**

   - Create a GET endpoint in the following format

   ```
   GET {yourBaseUrl}/api/Ready
   ```

   - The game engine will ping each registered player for a HTTP success status code
   - Once both players return a success, the game initialization will begin

2. **Initialization**:

   - Create a POST endpoint in the following format

   ```
   POST {yourBaseUrl}/api/GameInitialisation
   ```

   Request body example:

   ```json
   {
     "theBaseUrl": "https://gameengine.azurewebsites.net",
     "yourSecretKey": "your-secret-key-here",
     "yourType": "Red" // or "Blue"
   }
   ```

   - The secret key in this request will be used to authenticate your subsequent API calls
   - Store the game engine's base URL for submitting turns
   - Your team type (Red or Blue) determines which endpoints you'll use

3. **Bot Information Exchange**:

   - Create a GET endpoint to provide your bot identifiers

   ```
   GET {yourBaseUrl}/api/PlayerBotInformationExchange
   ```

   Response should include your three bots:

   ```json
   {
     "firstPlayerBot": {
       "botId": "unique-bot-id-1",
       "type": "Painter",
       "status": "Alive",
       "botIncapacitatedTimer": 0,
       "teamPlayerType": "Red"
     },
     "secondPlayerBot": {
       "botId": "unique-bot-id-2",
       "type": "Attack",
       "status": "Alive",
       "botIncapacitatedTimer": 0,
       "teamPlayerType": "Red"
     },
     "thirdPlayerBot": {
       "botId": "unique-bot-id-3",
       "type": "Defense",
       "status": "Alive",
       "botIncapacitatedTimer": 0,
       "teamPlayerType": "Red"
     }
   }
   ```

   - Store these bot IDs for use in your turn submissions
   - Each team must have exactly one bot of each type

### Game Loop:

- Receive board state updates via an endpoint:

```
POST {yourBaseUrl}/api/BoardUpdate
```

- Submit your turn via

```
POST {gameEngineBaseUrl}/api/RedInput
POST {gameEngineBaseUrl}/api/BlueInput
```

- Wait for the next board update and repeat

### API Endpoints

#### Game Initialization

```
POST /api/gameinitialisation
```

Request body:

```json
{
  "yourType": 0, // 0 for Red, 1 for Blue
  "theBaseUrl": "your_callback_url"
}
```

#### Get Bot Information

```
GET /api/gameinitialisationhandshake
```

Returns information about your team's bots.

#### Submit Turn

All requests are the same for the opposing colour, they just need their player type changed to blue

```
POST /api/RedInput
```

Request body:

```json
{
  "timeStamp": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "turnMoveSets": [
    {
      "playerBot": {
        "botId": "83f9262f-28f1-4703-ab1a-8cfd9e8249c9",
        "type": 0, // Painter Bot
        "status": 0,
        "botIncapacitatedTimer": 0,
        "teamPlayerType": 0 // Red Team
      },
      "moves": [
        {
          "action": 2, // Paint
          "position": {
            "x": 6,
            "y": 7
          },
          "targetPosition": null
        },
        {
          "action": 2, // Paint
          "position": {
            "x": 7,
            "y": 7
          },
          "targetPosition": null
        },
        {
          "action": 2, // Paint
          "position": {
            "x": 8,
            "y": 7
          },
          "targetPosition": null
        }
      ]
    },
    {
      "playerBot": {
        "botId": "a1b2c3d4-5678-90ab-cdef-11111222233",
        "type": 1, // Attack Bot
        "status": 0,
        "botIncapacitatedTimer": 0,
        "teamPlayerType": 0 // Red Team
      },
      "moves": [
        {
          "action": 3, // Wait
          "position": {
            "x": 1,
            "y": 7
          },
          "targetPosition": null
        },
        {
          "action": 3, // Wait
          "position": {
            "x": 1,
            "y": 6
          },
          "targetPosition": null
        },
        {
          "action": 0, // Strike
          "position": {
            "x": 1,
            "y": 5
          },
          "targetPosition": {
            "x": 1,
            "y": 4 // Target the Blue Attack Bot
          }
        }
      ]
    },
    {
      "playerBot": {
        "botId": "e5f6g7h8-9012-34ij-klmn-opqr56789012",
        "type": 2, // Defense Bot
        "status": 0,
        "botIncapacitatedTimer": 0,
        "teamPlayerType": 0 // Red Team
      },
      "moves": [
        {
          "action": 3, // Wait
          "position": {
            "x": 5,
            "y": 9
          },
          "targetPosition": null
        },
        {
          "action": 1, // Defend
          "position": {
            "x": 6,
            "y": 9
          },
          "targetPosition": null
        }
      ]
    }
  ]
}
```

Uses the same format as the Red Team input.

#### Ready Check

```
GET /api/Ready
```

Check if the game server is ready to accept connections.

## Strategy Tips

1. **Territory Control**: The more squares you control, the better position you're in
2. **Coordinate Bot Actions**: Use your Attack bots to clear the way for your Painter bots
3. **Protect Key Areas**: Use Defense bots to safeguard important territory
4. **Plan Multiple Turns Ahead**: Anticipate your opponent's moves and counter them
5. **Block Enemy Expansion**: Position your bots to obstruct enemy movement
6. **Balance Offense and Defense**: Don't focus exclusively on one strategy

## Common Errors

- **400 Bad Request**: Your turn submission is invalid (check bot IDs, positions, or move legality)
- **401 Unauthorized**: Your authentication is incorrect (check your secret key)
- **500 Internal Server Error**: There's an issue with the game server

## Common Issues and Troubleshooting

### Sign-up Issues

- **400 Bad Request on Sign-up**: Check that your team name is unique and your base URL is valid
- **Game engine can't reach your endpoints**: Ensure ngrok is running and the URL is correct
- **Endpoints not being called**: Verify all required endpoints are implemented and returning correct status codes

### Local Development Issues

- **ngrok session expired**: Restart ngrok and sign up again with the new URL
- **"Tunnel not found" errors**: Make sure ngrok is running on the correct port
- **Can't receive game engine calls**: Check firewall settings and ensure your server is listening on all interfaces (0.0.0.0), not just localhost

### Game Play Issues

- **Never receive board updates**: Ensure your `/api/Ready` endpoint returns 200 OK
- **Moves rejected**: Verify bot IDs match those provided in initialization
- **Authentication failures**: Confirm you're using the secret key from the current game, not a previous one

## Getting Started

1. Choose your programming language and set up HTTP client libraries
2. If developing locally, set up ngrok to expose your endpoints
3. **Register your player** by calling the `/api/SignUp` endpoint with your team name and base URL
4. Implement the required endpoints:
   - `GET /api/Ready` - Return 200 when ready to play
   - `POST /api/GameInitialisation` - Receive game configuration and secret key
   - `GET /api/PlayerBotInformationExchange` - Provide your bot IDs
   - `POST /api/BoardUpdate` - Receive board state and submit turns
5. Wait for the game engine to initiate a match
6. Start with simple strategies and iterate
7. Monitor game state updates to adapt your strategy

## Local Development with ngrok

When developing your CodeBlock player locally, you'll need to expose your local endpoints to the internet so the game engine can communicate with your player. Since Azure App Service (where the game engine is hosted) cannot directly point to localhost URLs, you'll need to use a tunneling service like ngrok.

### Why ngrok?

- The game engine needs to send HTTP requests to your player endpoints (e.g., `/api/Ready`, `/api/GameInitialisation`, `/api/BoardUpdate`)
- Your local development server (running on localhost) is not accessible from the internet
- ngrok creates a secure tunnel from a public URL to your localhost

### Setting up ngrok

1. **Install ngrok**: Download and install ngrok from [https://ngrok.com](https://ngrok.com)

2. **Start your local server**: Run your CodeBlock player on a specific port (e.g., 5000)

3. **Create an ngrok tunnel**:

   ```bash
   ngrok http 5000
   ```

4. **Use the ngrok URL**: ngrok will provide you with a public URL like:

   ```
   https://abc123.ngrok.io
   ```

5. **Sign up with the game engine**: Use your ngrok URL when registering:

   ```bash
   POST {gameEngineBaseUrl}/api/SignUp
   Content-Type: application/json

   {
     "playersBaseUrl": "https://abc123.ngrok.io",
     "playersTeamName": "YourTeamName"
   }
   ```

### Important Notes

- The ngrok URL changes each time you restart ngrok (unless you have a paid account)
- Keep ngrok running while testing your player
- You can inspect incoming requests in the ngrok web interface (http://localhost:4040)
- Make sure your local server handles all required endpoints before signing up
- If your ngrok session expires or you get a new URL, you'll need to sign up again with the new URL

## Azure Deployment

Once you've tested your CodeBlock player locally and are ready for production, you can deploy it to Azure. When deployed to Azure, you no longer need ngrok as your application will have a public URL accessible from the internet.

### Deployment Steps

1. **Create an Azure App Service**: Set up a new App Service in your Azure subscription

2. **Deploy your code**: Use your preferred deployment method:

   - Azure CLI
   - GitHub Actions
   - Visual Studio/VS Code
   - ZIP deployment

3. **Update your player registration**: Sign up with your Azure URL:

   ```bash
   POST {gameEngineBaseUrl}/api/SignUp
   Content-Type: application/json

   {
     "playersBaseUrl": "https://yourapp.azurewebsites.net",
     "playersTeamName": "YourTeamName"
   }
   ```

4. **Configure application settings**: Set any environment variables or configuration needed for your player

5. **Monitor your application**: Use Azure Application Insights to track performance and debug issues

### Key Differences from Local Development

- **No tunnel required**: Direct communication between game engine and your player
- **Persistent URL**: Your URL remains constant, unlike ngrok
- **Production-ready**: Handle multiple games simultaneously
- **Azure authentication**: Ensure your App Service is publicly accessible (no authentication required for the game endpoints)
- **Stable registration**: No need to re-register due to URL changes

Good luck, and may the best team win!
