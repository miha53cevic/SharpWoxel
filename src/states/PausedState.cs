using glObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.gui;
using SharpWoxel.util;

namespace SharpWoxel.states
{
    class PausedState : State
    {
        private Texture _buttonTexture;

        public PausedState(Game game) 
            : base(game)
        {
            _buttonTexture = Texture.LoadFromFile("../../../res/test.png");
        }

        public override void OnExit()
        {
        }

        public override void OnRenderFrame(double deltaTime)
        {
            _buttonTexture.Use(TextureUnit.Texture0);
            var size = new Vector2(200, 200);
            var position = new Vector2(_gameRef.RenderResolution.X / 2 - (size.X / 2), _gameRef.RenderResolution.Y / 2 - (size.Y / 2));
            GUI.GetInstance().Rectangle.Render(ShaderLoader.GetInstance().GetShader("gui"), position, size);
        }

        public override void OnUpdateFrame(double deltaTime)
        {
            var input = _gameRef.KeyboardState;
            if (input.IsKeyPressed(Keys.Escape))
            {
                StateManager.GetInstance().Pop();
            }
        }

        public override void Pause()
        {
        }

        public override void Resume()
        {
        }

        public override void Setup()
        {
            // Release the mouse from the window
            _gameRef.CursorState = CursorState.Normal;
        }
    }
}
