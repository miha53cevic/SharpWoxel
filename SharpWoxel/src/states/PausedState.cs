using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.GLObjects;
using SharpWoxel.gui;
using SharpWoxel.util;

namespace SharpWoxel.states;

internal class PausedState(Game game) : State(game)
{
    private readonly Texture _buttonTexture = Texture.LoadFromFile("../../../res/paused_font_goudystout.png");
    private readonly Rect _uiPause = Gui.CreateRect();

    public override void OnExit()
    {
    }

    public override void Setup()
    {
        // Release the mouse from the window
        GameRef.CursorState = CursorState.Normal;

        _uiPause.Position = new Vector2i(GameRef.RenderResolution.X / 2, GameRef.RenderResolution.Y / 2);
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
        var input = GameRef.KeyboardState;
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