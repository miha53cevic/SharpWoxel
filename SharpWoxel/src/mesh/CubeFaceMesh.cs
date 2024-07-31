using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;

namespace SharpWoxel.mesh;

internal class CubeFaceMesh : IMesh
{
    public enum Face
    {
        Left = 0,
        Right = 1,
        Front = 2,
        Back = 3,
        Top = 4,
        Bottom = 5,
    }

    private static readonly uint[] Indicies =
    [
        0, 1, 3,
        3, 1, 2
    ];

    private static readonly float[] TextureCoordinates =
    [
        0.0f, 0.0f,
        0.0f, 1.0f,
        1.0f, 1.0f,
        1.0f, 0.0f
    ];

    private readonly Ebo _ebo;
    private readonly Vao _vao;

    public CubeFaceMesh(Face face)
    {
        _vao = new Vao();
        var vertVbo = new Vbo();
        var texVbo = new Vbo();
        _ebo = new Ebo();

        _vao.Bind();
        vertVbo.SetBufferData(GetCubeFaceVerticies(face), BufferUsageHint.StaticDraw);
        texVbo.SetBufferData(TextureCoordinates, BufferUsageHint.StaticDraw);
        _ebo.SetElementBufferData(Indicies, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(vertVbo, 0, 3, 3 * sizeof(float), 0);
        _vao.DefineVertexAttribPointer(texVbo, 1, 2, 2 * sizeof(float), 0);
    }

    public void Render()
    {
        _vao.Bind();
        GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
    }

    public static float[] GetCubeFaceVerticies(Face face)
    {
        return face switch
        {
            Face.Top => [0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0],
            Face.Bottom => [0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1],
            Face.Left => [0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1],
            Face.Right => [1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0],
            Face.Front => [0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1],
            Face.Back => [1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0],
            _ => throw new Exception("Invalid Face for CubeFace given")
        };
    }
}