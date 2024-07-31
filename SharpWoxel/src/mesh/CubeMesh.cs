using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;

namespace SharpWoxel.mesh;

internal class CubeMesh : IMesh
{
    private static readonly float[] Verticies =
    [
        // Back face
        1f, 1f, 0f,
        1f, 0f, 0f,
        0f, 0f, 0f,
        0f, 1f, 0f,

        // Front face
        0f, 1f, 1f,
        0f, 0f, 1f,
        1f, 0f, 1f,
        1f, 1f, 1f,

        // Right face
        1f, 1f, 1f,
        1f, 0f, 1f,
        1f, 0f, 0f,
        1f, 1f, 0f,

        // Left Face
        0f, 1f, 0f,
        0f, 0f, 0f,
        0f, 0f, 1f,
        0f, 1f, 1f,

        // Top face
        0f, 1f, 1f,
        1f, 1f, 1f,
        1f, 1f, 0f,
        0f, 1f, 0f,

        // Bottom face
        0f, 0f, 1f,
        0f, 0f, 0f,
        1f, 0f, 0f,
        1f, 0f, 1f
    ];

    private static readonly float[] TextureCoordinates =
    [
        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f
    ];

    private static readonly uint[] Indicies =
    [
        0, 1, 3,
        3, 1, 2,
        4, 5, 7,
        7, 5, 6,
        8, 9, 11,
        11, 9, 10,
        12, 13, 15,
        15, 13, 14,
        16, 17, 19,
        19, 17, 18,
        20, 21, 23,
        23, 21, 22
    ];

    private readonly Ebo _ebo;
    private readonly Vao _vao;

    public CubeMesh()
    {
        _vao = new Vao();
        var vertVbo = new Vbo();
        var texVbo = new Vbo();
        _ebo = new Ebo();

        _vao.Bind();
        vertVbo.SetBufferData(Verticies, BufferUsageHint.StaticDraw);
        texVbo.SetBufferData(TextureCoordinates, BufferUsageHint.StaticDraw);
        _ebo.SetElementBufferData(Indicies, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(vertVbo, 0, 3, 3 * sizeof(float), 0);
        _vao.DefineVertexAttribPointer(texVbo, 1, 2, 2 * sizeof(float), 0);
    }

    public void Render()
    {
        _vao.Bind();
        GL.DrawElements(BeginMode.Lines, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
    }
}