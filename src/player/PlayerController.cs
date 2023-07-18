using SharpWoxel.util;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SharpWoxel.player
{
    class PlayerController
    {
        public float PlayerSpeed { get; set; }
        public float Sensetivity { get; set; }
        public Camera Camera { get; private set; }

        public PlayerController(Camera camera)
        {
            
            Camera = camera;
            PlayerSpeed = 10.0f;
            Sensetivity = 0.25f;
        }

        private void HandleInput(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
        {
            // Keyboard inputs
            if (keyInput.WasKeyDown(Keys.W))
            {
                Camera.Position += Camera.Front * (float)deltaTime * PlayerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.S))
            {
                Camera.Position -= Camera.Front * (float)deltaTime * PlayerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.A))
            {
                Camera.Position -= Camera.Right * (float)deltaTime * PlayerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.D))
            {
                Camera.Position += Camera.Right * (float)deltaTime * PlayerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.Space))
            {
                Camera.Position += Camera.Up * (float)deltaTime * PlayerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.LeftControl))
            {
                Camera.Position -= Camera.Up * (float)deltaTime * PlayerSpeed;
            }

            // Mouse inputs
            Camera.Yaw += mouseInput.Delta.X * Sensetivity;
            Camera.Pitch -= mouseInput.Delta.Y * Sensetivity;
        }

        public void Update(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
        {
            HandleInput(deltaTime, keyInput, mouseInput);
        }
    }
}
