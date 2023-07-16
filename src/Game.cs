using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using glObjects;
using SharpWoxel.util;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.Player;

namespace SharpWoxel
{
    public class Game : GameWindow
    {
        private readonly float[] verticies = {
            -0.5f,  0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.5f,  0.5f, 0.0f
        };
        private readonly uint[] indicies = {
            0, 1, 2,
            2, 3, 0
        };
        // bottom left is 0,0 and top right is 1,1 in opengl
        private readonly float[] texCords = {
            0.0f, 1.0f,
            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f

        };
        private Shader shader;
        private Camera camera;
        private SimpleEntity testEntity;
        private PlayerController playerController;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            shader = new Shader("../../../shaders/basic.vert", "../../../shaders/basic.frag");
            testEntity = new SimpleEntity("../../../res/test.png");
            camera = new Camera(new Vector3(0, 0, 0), (float)width / (float)height);
            playerController = new PlayerController(camera);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace); // Cull faces (render only triangles that are counter-clockwise)

            testEntity.SetVerticies(verticies, BufferUsageHint.StaticDraw);
            testEntity.SetTextureCoords(texCords, BufferUsageHint.StaticDraw);
            testEntity.SetIndicies(indicies, BufferUsageHint.StaticDraw);
            testEntity.Position += (0, 0, -1);
        }
        protected override void OnUnload()
        {
            base.OnUnload();

            shader.Dispose();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        // Rendering/Drawing updates
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render Code
            testEntity.Render(shader, playerController.camera);

            SwapBuffers();
        }

        // Fixed updates
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Lock cursor (and hide it) to the screen if the window is in focus
            if (IsFocused)
            {
                CursorState = CursorState.Grabbed;
            }
            else
            {
                CursorState = CursorState.Normal;
                return; // exit function if window is not in focus
            }

            // Exit with escape
            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            playerController.Update(args.Time, KeyboardState, MouseState);
        }
    }
}