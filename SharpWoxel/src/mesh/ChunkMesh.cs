using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;

namespace SharpWoxel.mesh;

internal class ChunkMesh : IMesh
{
    private readonly Ebo _ebo = new();
    private readonly Vbo _textureCoordsVbo = new();
    private readonly Vao _vao = new();
    private readonly Vbo _verticiesVbo = new();

    public ChunkMesh(float[] verticies, uint[] indicies, float[] textureCoords, BufferUsageHint usage)
    {
        RebuildMesh(verticies, indicies, textureCoords, usage);
    }

    public void Render()
    {
        _vao.Bind();
        GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
    }

    public void RebuildMesh(float[] verticies, uint[] indicies, float[] textureCoords, BufferUsageHint usage)
    {
        SetVerticies(verticies, usage);
        SetIndicies(indicies, usage);
        SetTextureCoords(textureCoords, usage);
    }

    private void SetVerticies(float[] data, BufferUsageHint usage)
    {
        _vao.Bind();
        _verticiesVbo.SetBufferData(data, usage);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_verticiesVbo, 0, 3, 3 * sizeof(float), 0);
    }

    private void SetIndicies(uint[] data, BufferUsageHint usage)
    {
        _vao.Bind();
        _ebo.SetElementBufferData(data, usage);
        _vao.Unbind();
    }

    private void SetTextureCoords(float[] data, BufferUsageHint usage)
    {
        _vao.Bind();
        _textureCoordsVbo.SetBufferData(data, usage);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_textureCoordsVbo, 1, 2, 2 * sizeof(float), 0);
    }
}