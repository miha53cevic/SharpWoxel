using OpenTK.Graphics.OpenGL4;

namespace SharpWoxel.GLObjects;

internal class Ebo
{
    private readonly int _ebo = GL.GenBuffer();

    public int Size { get; private set; }

    public void SetElementBufferData(uint[] data, BufferUsageHint usage)
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, usage);

        Size = data.Length;
    }
}