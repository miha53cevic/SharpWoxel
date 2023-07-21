using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using glObjects;
using SharpWoxel.util;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.player;
using SharpWoxel.world.terrain;

namespace SharpWoxel
{
    public class Game : GameWindow
    {
        private bool _wireframe = false;
        private Shader shader;
        private Camera camera;
        private SimpleEntity testEntity;
        private PlayerController playerController;
        private Terrain terrain;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            shader = new Shader("../../../shaders/basic.vert", "../../../shaders/basic.frag");
            camera = new Camera(new Vector3(0, 0, 0), (float)width / (float)height);
            testEntity = new SimpleEntity("../../../res/test.png");
            playerController = new PlayerController(camera);
            terrain = new FlatTerrain(new Vector3i(2, 1, 2), new Vector3i(32, 32, 32));
        }

        private void ToggleWireFrame()
        {
            _wireframe = !_wireframe;

            if (_wireframe) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace); // Cull faces (render only triangles that are counter-clockwise)

            testEntity.SetVerticies(Cube.verticies, BufferUsageHint.StaticDraw);
            testEntity.SetTextureCoords(Cube.textureCoordinates, BufferUsageHint.StaticDraw);
            testEntity.SetIndicies(Cube.indicies, BufferUsageHint.StaticDraw);
            testEntity.Position += (0, 0, -3);
            terrain.GenerateTerrain();
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

            Title = string.Format("SharpWoxel - FPS: {0:0.00}", 1.0f / args.Time);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render Code
            testEntity.Render(shader, playerController.Camera);
            terrain.Render(shader, playerController.Camera);

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

            if (input.IsKeyPressed(Keys.Tab))
            {
                ToggleWireFrame();
            }
            
            playerController.Update(args.Time, KeyboardState, MouseState);
        }
    }
}