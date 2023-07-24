
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.imgui;

namespace SharpWoxel.states
{
    class ImGuiOverlayState : State
    {
        public ImGuiOverlayState(Game game) : base(game)
        {
        }

        public override void OnExit()
        {
        }

        public override void OnRenderFrame(double deltaTime)
        {
            ImGuiSingleton.GetInstance().Render();
        }

        public override void OnUpdateFrame(double deltaTime)
        {
            ImGuiSingleton.GetInstance().Update(_gameRef, deltaTime);

            var input = _gameRef.KeyboardState;
            if (input.IsKeyPressed(Keys.O))
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
