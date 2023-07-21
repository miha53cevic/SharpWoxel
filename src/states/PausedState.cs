
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SharpWoxel.states
{
    class PausedState : State
    {
        public PausedState(Game game) 
            : base(game)
        {
        }

        public override void OnExit()
        {
            _gameRef.ResetClearColour();
        }

        public override void OnRenderFrame(double deltaTime)
        {
        }

        public override void OnUpdateFrame(double deltaTime)
        {
            var input = _gameRef.KeyboardState;
            if (input.IsKeyPressed(Keys.Escape))
            {
                _gameRef.GetStateManager().Pop();
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

            _gameRef.SetClearColour(51, 51, 51, 255);
        }
    }
}
