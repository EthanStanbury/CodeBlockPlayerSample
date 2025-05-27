namespace CodeBlock.PlayerSample.Models;

public record Position(int X, int Y)
{
    public override string ToString()
    {
        return $"Position: ({X},{Y})";
    }
}