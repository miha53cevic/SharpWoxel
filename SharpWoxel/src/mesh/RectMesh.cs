using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;

namespace SharpWoxel.mesh;

internal class RectMesh : IMesh
{
    // Verticies and texture coords are the same in the current order
    private static readonly float[] Verticies =
    [
        0f, 1f,
        0f, 0f,
        1f, 0f,
        1f, 1f
    ];

    private static readonly uint[] Indicies =
    [
        0, 1, 3,
        3, 1, 2
    ];

    private readonly Ebo _ebo;
    private readonly Vbo _textureCoordinatesVbo;
    private readonly Vao _vao;

    public RectMesh()
    {
        _vao = new Vao();
        var verticiesVbo = new Vbo();
        _textureCoordinatesVbo = new Vbo();
        _ebo = new Ebo();

        _vao.Bind();
        verticiesVbo.SetBufferData(Verticies, BufferUsageHint.StaticDraw);
        _ebo.SetElementBufferData(Indicies, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(verticiesVbo, 0, 2, 2 * sizeof(float), 0); // verticies
        _vao.DefineVertexAttribPointer(verticiesVbo, 1, 2, 2 * sizeof(float),
            0); // textureCoordinates for normal texture are the same as verticies
    }

    // Set custom texture coordinates when using TextureAtlas 
    public void SetTextureCoordinates(float[] textureCoordinates)
    {
        _vao.Bind();
        _textureCoordinatesVbo.SetBufferData(textureCoordinates, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_textureCoordinatesVbo, 1, 2, 2 * sizeof(float), 0);
    }

    public void Render()
    {
        _vao.Bind();
        GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
    }
}