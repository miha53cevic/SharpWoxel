using OpenTK.Graphics.OpenGL4;
using SharpWoxel.Util;
using SharpWoxel.GLO;

namespace SharpWoxel.Entities;

    class ChunkEntity : Entity
{
    private readonly VAO _vao;
    private readonly EBO _ebo;
    private readonly VBO _verticiesVBO;
    private readonly VBO _textureCoordsVBO;

    public static readonly GLO.TextureAtlas TexAtlas; // all the chunks use the same atlas

    static ChunkEntity()
    {
        TexAtlas = new TextureAtlas("../../../res/textureAtlas.png", 2048, 256);
    }

    public ChunkEntity()
    {
        _vao = new VAO();
        _ebo = new EBO();
        _verticiesVBO = new VBO();
        _textureCoordsVBO = new VBO();
    }

    public void SetVerticies(float[] data, BufferUsageHint usage)
    {
        _vao.Bind();
        _verticiesVBO.SetBufferData(data, usage);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_verticiesVBO, 0, 3, 3 * sizeof(float), 0);
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
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_textureCoordsVBO, 1, 2, 2 * sizeof(float), 0);
    }

    public override void Render(GLO.Shader shader, Camera camera)
    {
        shader.Use();
        TexAtlas.Use(TextureUnit.Texture0);

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
