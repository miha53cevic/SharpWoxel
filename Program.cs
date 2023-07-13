
namespace SharpWoxel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "SharpWoxel"))
            {
                game.Run();
            }
        }
    }
}