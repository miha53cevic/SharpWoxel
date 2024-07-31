using OpenTK.Graphics.OpenGL4;

namespace SharpWoxel.GLObjects;

internal class Vao
{
    private readonly int _vao = GL.GenVertexArray(); // readonly: can only be assigned in field or constructor

    public void Bind()
    {
        GL.BindVertexArray(_vao);
    }

    public void Unbind()
    {
        GL.BindVertexArray(0);
    }

    // size is vector size (vec3 has 3, vec2 has 2)
    public void DefineVertexAttribPointer(Vbo vbo, int attributeId, int size, int stride, int offset)
    {
        Bind();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.GetVbo());
        GL.VertexAttribPointer(attributeId, size, VertexAttribPointerType.Float, false, stride, offset);
        GL.EnableVertexAttribArray(attributeId);
        Unbind();
    }

    public int GetVao()
    {
        return _vao;
    }
}