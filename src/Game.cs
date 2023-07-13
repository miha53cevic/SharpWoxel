using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using glObjects;

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
        private VAO vao;
        private VBO vbo;
        private EBO ebo;
        private Shader shader;
        private VBO vboTexCords;
        private Texture texture;

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            vao = new VAO();
            vbo = new VBO();
            ebo = new EBO();
            vboTexCords = new VBO();
            shader = new Shader("../../../shaders/basic.vert", "../../../shaders/basic.frag");
            texture = Texture.LoadFromFile("../../../res/test.png");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Bind VAO
            vao.Bind();

            // add VBO
            vbo.SetBufferData(verticies, BufferUsageHint.StaticDraw);

            vboTexCords.SetBufferData(texCords, BufferUsageHint.StaticDraw);
            vboTexCords.DefineVertexAttribPointer(1, 2, 2 * sizeof(float), 0);

            // Create EBO (an EBO can only be bound if a VAO is bound!)
            ebo.SetElementBufferData(indicies, BufferUsageHint.StaticDraw);

            // VertexAttribPointer index is the index of the attribute in the shader
            // One VAO has multiple AttribPointers to multiple or the same VBO
            vbo.DefineVertexAttribPointer(0, 3, 3 * sizeof(float), 0);


            vao.Unbind();
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

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Render Code
            shader.Use();
            vao.Bind();
            texture.Use(TextureUnit.Texture0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(BeginMode.Triangles, ebo.Size, DrawElementsType.UnsignedInt, 0);
            vao.Unbind();

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