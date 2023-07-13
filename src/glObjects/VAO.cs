using OpenTK.Graphics.OpenGL4;

namespace glObjects
{
    class VAO
    {
        private readonly int _VAO; // readonly: can only be assigned in field or constructor
        
        public VAO() 
        {
            _VAO = GL.GenVertexArray();
        }

        public void Bind() { GL.BindVertexArray(_VAO); }
        public void Unbind() { GL.BindVertexArray(0); }
        public int GetVAO() { return _VAO; }
    }
}
