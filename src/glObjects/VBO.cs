using OpenTK.Graphics.OpenGL4;

namespace glObjects
{
    class VBO
    {
        private readonly int _VBO;

        public VBO()
        {
            _VBO = GL.GenBuffer();
        }

        public void SetBufferData(float[] data, BufferUsageHint usage)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, usage);
        }

        // size is vector size (vec3 has 3, vec2 has 2)
        public void DefineVertexAttribPointer(int attributeID, int size, int stride, int offset)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);

            GL.VertexAttribPointer(attributeID, size, VertexAttribPointerType.Float, false, stride, offset);
            GL.EnableVertexAttribArray(attributeID);
        }

        public int GetVBO() { return _VBO; }
    }
}
