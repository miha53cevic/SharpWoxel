using OpenTK.Graphics.OpenGL4;
using glObjects;
using OpenTK.Mathematics;

namespace SharpWoxel.gui
{
    class Rect
    {
        private VAO _vao;
        private Matrix4 _projection;

        // Verticies and texture coords are the same in the current order
        private static readonly float[] _verticies = {
                0.0f, 1.0f,
                0.0f, 0.0f,
                1.0f, 1.0f,

                1.0f, 1.0f,
                0.0f, 0.0f,
                1.0f, 0.0f
            };

        public Rect(Matrix4 projection)
        {
            _vao = new VAO();
            _projection = projection;

            PrepRenderData();
        }

        private void PrepRenderData()
        {
            _vao.Bind();
            VBO vbo = new VBO();
            vbo.SetBufferData(_verticies, BufferUsageHint.StaticDraw);
            vbo.DefineVertexAttribPointer(0, 2, 2 * sizeof(float), 0); // verticies
            vbo.DefineVertexAttribPointer(1, 2, 2 * sizeof(float), 0); // textureCoordinates
            _vao.Unbind();
        }

        // Note: Starting position is bottom left cornor and is regarded as (0,0)
        public void Render(Shader shader, Vector2 position, Vector2 size, float rotation = 0)
        {
            var projection = _projection;

            // OpenTK - Scale -> Rotation -> Transform
            var model = Matrix4.Identity;
            model *= Matrix4.CreateScale(size.X, size.Y, 1.0f);
            model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation));
            model *= Matrix4.CreateTranslation(new Vector3(position.X, position.Y, 0.0f));

            shader.Use();
            // OpenTK is row-based, so we multiply like this instead of projection*matrix which would be in c++
            shader.SetMatrix4(shader.GetUniformLocation("modelProjection"), model * projection); 

            _vao.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticies.Length);
            _vao.Unbind();
        }
    }
}
