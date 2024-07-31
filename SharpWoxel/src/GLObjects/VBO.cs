using OpenTK.Graphics.OpenGL4;

namespace SharpWoxel.GLObjects;

internal class Vbo
{
    private readonly int _vbo = GL.GenBuffer();

    public void SetBufferData(float[] data, BufferUsageHint usage)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, usage);
    }

    public int GetVbo()
    {
        return _vbo;
    }
}