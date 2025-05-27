namespace CodeBlock.PlayerSample.Models;

public enum PlayerType
{
    Red, 
    Blue,
    Neutral
}

public enum BotType
{
    Painter, 
    Attack,
    Defense,
    Empty
}

public enum MoveAction
{
   Strike, 
   Defend, 
   Paint, 
   Wait,
   ChangeBotType
}

public enum BotStatus
{
    Alive,
    Incapacitated,
    Unassigned,
    Collided
}

public enum GameEngineEndpoint
{
    SignUp,
    RedInput,
    BlueInput,
}