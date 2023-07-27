using glObjects;
using OpenTK.Graphics.OpenGL4;
using SharpWoxel.util;

namespace SharpWoxel.entities
{
    class VoxelOutline : Entity
    {
        private readonly VAO _vao;
        private readonly VBO _vbo;
        private readonly EBO _ebo;

        public static float[] Verticies = {
            0f,1f,1f,
            0f,1f,0f,
            1f,1f,0f,
            1f,1f,1f,

            0f,0f,1f,
            0f,0f,0f,
            1f,0f,0f,
            1f,0f,1f
        };

        public static uint[] Indicies = {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
            0, 4,
            1, 5,
            2, 6,
            3, 7
        };

        public VoxelOutline()
        {
            _vao = new VAO();
            _vbo = new VBO();
            _ebo = new EBO();

            _vao.Bind();
            _vbo.SetBufferData(Verticies, BufferUsageHint.StaticDraw);
            _ebo.SetElementBufferData(Indicies, BufferUsageHint.StaticDraw);
            _vao.Unbind();

            _vao.DefineVertexAttribPointer(_vbo, 0, 3, 3 * sizeof(float), 0);

            Scale *= (1.01f, 1.01f, 1.01f);
        }

        public override void Render(Shader shader, Camera camera)
        {
            shader.Use();
            var model = Maths.CreateTransformationMatrix(
                Position,
                Rotation,
                Scale
            );
            shader.SetMatrix4(shader.GetUniformLocation("mvp"), Maths.CreateMVPMatrix(camera, model));

            _vao.Bind();
            GL.DrawElements(BeginMode.Lines, _ebo.Size, DrawElementsType.UnsignedInt, 0);
            _vao.Unbind();
        }

        public override void Update(double deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
