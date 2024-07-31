namespace SharpWoxel;

public static class Program
{
    public static void Main(string[] args)
    {
        using var game = new Game(1280, 720, "SharpWoxel");
        game.Run();
    }
}