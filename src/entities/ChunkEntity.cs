using glObjects;
using OpenTK.Graphics.OpenGL4;
using SharpWoxel.util;

namespace SharpWoxel.entities
{
    class ChunkEntity : Entity
    {
        private readonly TextureAtlas _atlas;
        private readonly VAO _vao;
        private readonly EBO _ebo;
        private readonly List<VBO> _vbos;

        public ChunkEntity(string atlasTexturePath, int imageSize, int individualTextureSize)
        {
            _atlas = new TextureAtlas(atlasTexturePath, imageSize, individualTextureSize);
            _vao = new VAO();
            _ebo = new EBO();
            _vbos = new List<VBO>();
        }

        public void SetVerticies(float[] data, BufferUsageHint usage)
        {
            _vao.Bind();
            VBO vbo = new VBO();

            vbo.SetBufferData(data, usage);
            vbo.DefineVertexAttribPointer(0, 3, 3 * sizeof(float), 0);
            _vbos.Add(vbo);
            _vao.Unbind();
        }
        public void SetIndicies(uint[] data, BufferUsageHint usage)
        {
            _vao.Bind();
            _ebo.SetElementBufferData(data, usage);
            _vao.Unbind();
        }

        public void SetTextureCoords(float[] data, BufferUsageHint usage)
        {
            _vao.Bind();
            VBO vbo = new VBO();
            vbo.SetBufferData(data, usage);
            vbo.DefineVertexAttribPointer(1, 2, 2 * sizeof(float), 0);
            _vbos.Add(vbo);
            _vao.Unbind();
        }

        public TextureAtlas GetTextureAtlas()
        {
            return _atlas;
        }

        public override void Render(Shader shader, Camera camera)
        {
            shader.Use();
            _atlas.Use(TextureUnit.Texture0);

            var model = Maths.CreateTransformationMatrix(
                Position,
                Rotation,
                Scale
            );
            shader.SetMatrix4(shader.GetUniformLocation("mvp"), Maths.CreateMVPMatrix(camera, model));

            _vao.Bind();
            GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
            _vao.Unbind();
        }

        public override void Update(double deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
