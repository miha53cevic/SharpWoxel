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
        private readonly VBO _verticiesVBO;
        private readonly VBO _textureCoordsVBO;

        public ChunkEntity(string atlasTexturePath, int imageSize, int individualTextureSize)
        {
            _atlas = new TextureAtlas(atlasTexturePath, imageSize, individualTextureSize);
            _vao = new VAO();
            _ebo = new EBO();
            _verticiesVBO = new VBO();
            _textureCoordsVBO = new VBO();
        }

        public void SetVerticies(float[] data, BufferUsageHint usage)
        {
            _vao.Bind();
            _verticiesVBO.SetBufferData(data, usage);
            _verticiesVBO.DefineVertexAttribPointer(0, 3, 3 * sizeof(float), 0);
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
            _textureCoordsVBO.SetBufferData(data, usage);
            _textureCoordsVBO.DefineVertexAttribPointer(1, 2, 2 * sizeof(float), 0);
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
