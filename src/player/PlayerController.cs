using SharpWoxel.util;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SharpWoxel.player
{
    class PlayerController
    {
        private float _playerSpeed = 10.0f;
        private float _sensetivity = 0.25f;
        public Camera camera { get; private set; }

        public PlayerController(Camera camera)
        {
            this.camera = camera;
        }

        public void Update(double deltaTime, KeyboardState keyInput, MouseState mouseInput)
        {
            // Keyboard inputs
            if (keyInput.WasKeyDown(Keys.W))
            {
                camera.Position += camera.Front * (float)deltaTime * _playerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * (float)deltaTime * _playerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * (float)deltaTime * _playerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.D))
            {
                camera.Position += camera.Right * (float)deltaTime * _playerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * (float)deltaTime * _playerSpeed;
            }

            if (keyInput.WasKeyDown(Keys.LeftControl))
            {
                camera.Position -= camera.Up * (float)deltaTime * _playerSpeed;
            }

            // Mouse inputs
            camera.Yaw += mouseInput.Delta.X * _sensetivity;
            camera.Pitch -= mouseInput.Delta.Y * _sensetivity; 
        }
    }
}
