using glObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.entities
{
    class SimpleEntity : IEntity
    {
        private readonly Texture _texture;
        private readonly VAO _vao;
        private readonly EBO _ebo;
        protected readonly List<VBO> _vbos;

        private bool _loadedEBO = false;
        private bool _loadedVerticies = false;
        private bool _loadedTextureCoords = false;

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public SimpleEntity(string texturePath)
        {
            _texture = Texture.LoadFromFile(texturePath);
            _vao = new VAO();
            _ebo = new EBO();
            _vbos = new List<VBO>();

            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);
        }

        public void SetVerticies(float[] data, BufferUsageHint usage)
        {
            _vao.Bind();
            VBO vbo = new VBO();
            vbo.SetBufferData(data, usage);
            // VertexAttribPointer index is the index of the attribute in the shader
            // One VAO has multiple AttribPointers to multiple or the same VBO
            // zamisli kao pointer da si napravil, a zna se da je to taj jer SetBufferData radi BindBuffer unutar Bindanog VAO
            vbo.DefineVertexAttribPointer(0, 3, 3 * sizeof(float), 0); // 3 jer je vec3 (x, y, z)
            _vbos.Add(vbo);
            _vao.Unbind();

            _loadedVerticies = true;
        }

        public void SetIndicies(uint[] data, BufferUsageHint usage)
        {
            // Create EBO (an EBO can only be bound if a VAO is bound!)
            _vao.Bind();
            _ebo.SetElementBufferData(data, usage);
            _vao.Unbind();

            _loadedEBO = true;
        }

        public void SetTextureCoords(float[] data, BufferUsageHint usage)
        {
            _vao.Bind();
            VBO vbo = new VBO();
            vbo.SetBufferData(data, usage);
            // zamisli kao pointer da si napravil, a zna se da je to taj jer SetBufferData radi BindBuffer unutar Bindanog VAO
            vbo.DefineVertexAttribPointer(1, 2, 2 * sizeof(float), 0); // 2 jer je vec2 (x, y)
            _vbos.Add(vbo);
            _vao.Unbind();

            _loadedTextureCoords = true;
        }

        // Can be overriden to add extra uniform variables or any extra stuff
        protected virtual void PrepRender(Shader shader, Camera camera)
        {
            shader.Use();
            _texture.Use(TextureUnit.Texture0);

            var model = Maths.CreateTransformationMatrix(
                Position,
                Rotation,
                Scale
            );
            shader.SetMatrix4(shader.GetUniformLocation("mvp"), Maths.CreateMVPMatrix(camera, model));
        }
        public virtual void Render(Shader shader, Camera camera)
        {
            if (!_loadedVerticies || !_loadedTextureCoords || !_loadedEBO)
                throw new Exception("[SimpleEntity]: Not fully initialized");

            PrepRender(shader, camera);

            _vao.Bind();
            GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
            _vao.Unbind();
        }

        public virtual void Update(double deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
