using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;

namespace SharpWoxel.mesh;

internal class CubeOutlineMesh : IMesh
{
    private static readonly float[] Verticies =
    [
        0f, 1f, 1f,
        0f, 1f, 0f,
        1f, 1f, 0f,
        1f, 1f, 1f,

        0f, 0f, 1f,
        0f, 0f, 0f,
        1f, 0f, 0f,
        1f, 0f, 1f
    ];

    private static readonly uint[] Indicies =
    [
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
    ];

    private readonly Ebo _ebo;
    private readonly Vao _vao;

    public CubeOutlineMesh()
    {
        _vao = new Vao();
        var vbo = new Vbo();
        _ebo = new Ebo();

        _vao.Bind();
        vbo.SetBufferData(Verticies, BufferUsageHint.StaticDraw);
        _ebo.SetElementBufferData(Indicies, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(vbo, 0, 3, 3 * sizeof(float), 0);
    }

    public void Render()
    {
        _vao.Bind();
        GL.DrawElements(BeginMode.Lines, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
    }
}