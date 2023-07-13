using OpenTK.Graphics.OpenGL4;

namespace glObjects
{
    class EBO
    {
        private readonly int _EBO;

        public EBO()
        {
            _EBO = GL.GenBuffer();
            Size = 0;
        }

        public void SetElementBufferData(uint[] data, BufferUsageHint usage)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, usage);

            Size = data.Length;
        }

        public int Size { get; private set; }
    }
}
