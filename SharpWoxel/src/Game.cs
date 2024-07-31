using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.gui;
using SharpWoxel.imgui;
using SharpWoxel.states;
using SharpWoxel.util;

namespace SharpWoxel;

internal class Game(int width, int height, string title) : GameWindow(GameWindowSettings.Default,
    new NativeWindowSettings { ClientSize = (width, height), Title = title })
{
    private bool _wireframe;

    public Vector2i RenderResolution = new(width, height);

    private void ToggleWireFrame()
    {
        _wireframe = !_wireframe;
        GL.PolygonMode(MaterialFace.FrontAndBack, _wireframe ? PolygonMode.Line : PolygonMode.Fill);
    }

    public void SetClearColour(int r, int g, int b, int a)
    {
        var red = r / 255.0f;
        var green = g / 255.0f;
        var blue = b / 255.0f;
        var alpha = a / 255.0f;
        GL.ClearColor(red, green, blue, alpha);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace); // Cull faces (render only triangles that are counter-clockwise)

        ImGuiSingleton.GetInstance().Load(Size.X, Size.Y);

        Gui.Init(RenderResolution.X, RenderResolution.Y);
        ShaderLoader.GetInstance().Load("../../../shaders/basic");
        ShaderLoader.GetInstance().Load("../../../shaders/gui");
        ShaderLoader.GetInstance().Load("../../../shaders/voxelOutline");

        StateManager.GetInstance().Add(new PlayingState(this));
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        ImGuiSingleton.GetInstance().Destroy();

        // Dispose of all loaded shaders
        ShaderLoader.GetInstance().Destroy();
        // Clear all states
        StateManager.GetInstance().Destroy();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);

        ImGuiSingleton.GetInstance().OnResize(e.Width, e.Height);
    }

    // Rendering/Drawing updates
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        Title = $"SharpWoxel - FPS: {1.0f / args.Time:0.00}";

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Render Code
        StateManager.GetInstance().GetActiveStates().ForEach(state => state.OnRenderFrame(args.Time));

        SwapBuffers();
    }

    // Fixed updates
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyPressed(Keys.Tab)) ToggleWireFrame();
        if (KeyboardState.IsKeyPressed(Keys.P))
            if (StateManager.GetInstance().GetCurrentState().GetType() != typeof(ImGuiOverlayState))
                StateManager.GetInstance().Add(new ImGuiOverlayState(this));

        // Fixed updates code
        StateManager.GetInstance().GetActiveStates().ForEach(state => state.OnUpdateFrame(args.Time));
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);

        ImGuiSingleton.GetInstance().OnTextInput((char)e.Unicode);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        ImGuiSingleton.GetInstance().OnMouseScroll(e.Offset);
    }
}