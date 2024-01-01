using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.ImGUI;

namespace SharpWoxel.States;

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

        //ImGuiSingleton.GetInstance().AddDemoWindow();

        // Very simple example window
        /*ImGuiSingleton.GetInstance().AddRenderFunction(controller =>
        {
            // Must use if with Begin() for root window, rest is inside of the if
            // BeginChild() opens child windows inside the rooot.
            // If you don't want something then don't render it (bool values to check)
            if (ImGui.Begin("My first window"))
            {
                ImGui.Text("Hello Dear ImGui");
                float a = 1.0f;
                ImGui.SliderFloat("slider1", ref a, 0.0f, 2.0f);
            }
            ImGui.End();
        });*/
    }
}
