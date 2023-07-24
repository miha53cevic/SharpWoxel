﻿namespace SharpWoxel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (Game game = new Game(1280, 720, "SharpWoxel"))
            {
                game.Run();
            }
        }
    }
}