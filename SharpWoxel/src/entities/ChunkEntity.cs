using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;
using SharpWoxel.util;

namespace SharpWoxel.entities;

internal class ChunkEntity : Entity
{
    public static readonly TextureAtlas TexAtlas; // all the chunks use the same atlas
    private readonly Ebo _ebo = new();
    private readonly Vbo _textureCoordsVbo = new();
    private readonly Vao _vao = new();
    private readonly Vbo _verticiesVbo = new();

    static ChunkEntity()
    {
        TexAtlas = new TextureAtlas("../../../res/textureAtlas.png", 2048, 256);
    }

    public void SetVerticies(float[] data, BufferUsageHint usage)
    {
        _vao.Bind();
        _verticiesVbo.SetBufferData(data, usage);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_verticiesVbo, 0, 3, 3 * sizeof(float), 0);
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
        _textureCoordsVbo.SetBufferData(data, usage);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_textureCoordsVbo, 1, 2, 2 * sizeof(float), 0);
    }

    public override void Render(Shader shader, Camera camera)
    {
        shader.Use();
        TexAtlas.Use(TextureUnit.Texture0);

        var model = Maths.CreateTransformationMatrix(
            Position,
            Rotation,
            Scale
        );
        shader.SetMatrix4(shader.GetUniformLocation("mvp"), Maths.CreateMvpMatrix(camera, model));

        _vao.Bind();
        GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
    }

    public override void Update(double deltaTime)
    {
        throw new NotImplementedException();
    }
}