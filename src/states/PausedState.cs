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
        private Rect _uiPause = GUI.CreateRect();

        public PausedState(Game game) 
            : base(game)
        {
            _buttonTexture = Texture.LoadFromFile("../../../res/paused_font_goudystout.png");
        }

        public override void OnExit()
        {
        }

        public override void Setup()
        {
            // Release the mouse from the window
            _gameRef.CursorState = CursorState.Normal;

            _uiPause.Position = new Vector2i(_gameRef.RenderResolution.X / 2, _gameRef.RenderResolution.Y / 2);
            _uiPause.Size = new Vector2i(600, 200);
            _uiPause.CenterOnPosition();
        }

        public override void OnRenderFrame(double deltaTime)
        {
            _buttonTexture.Use(TextureUnit.Texture0);
            _uiPause.Render(ShaderLoader.GetInstance().GetShader("gui"));
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
    }
}
