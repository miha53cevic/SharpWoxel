using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;

namespace SharpWoxel.src
{
    public class Game : GameWindow
    {
        private readonly float[] triangleVerticies = {
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.0f,  0.5f, 0.0f
        };
        private readonly uint[] triangleIndicies = {
            0, 1, 2
        };
        private int VAO = -1;
        private int VBO = -1;
        private int EBO = -1;
        private Shader? shader;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Bind VAO
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Create VBO
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, triangleVerticies.Length * sizeof(float), triangleVerticies, BufferUsageHint.StaticDraw);

            // Create EBO (an EBO can only be bound if a VAO is bound!)
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, triangleIndicies.Length * sizeof(uint), triangleIndicies, BufferUsageHint.StaticDraw);

            // VertexAttribPointer index is the index of the attribute in the shader
            // One VAO has multiple AttribPointers to multiple or the same VBO
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0); // Enable the attribute (by default they are all disabled)

            shader = new Shader("../../../shaders/basic.vert", "../../../shaders/basic.frag");
        }
        protected override void OnUnload()
        {
            base.OnUnload();

            shader?.Dispose();
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

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Render Code
            shader?.Use();
            GL.BindVertexArray(VAO);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(BeginMode.Triangles, triangleIndicies.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        // Fixed updates
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}