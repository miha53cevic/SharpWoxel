using OpenTK.Graphics.OpenGL4;

namespace SharpWoxel
{
    class Shader : IDisposable
    {
        private int Handle = -1;
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexShaderSrc = File.ReadAllText(vertexPath);
            string fragmentShaderSrc = File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSrc);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSrc);


            int success;
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);

            // Cleanup, since they are linked now to the shader program
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If disposed is called twice, only run if value has not been already disposed
            if (!disposedValue)
            {
                // If the user called Dispose() on the object
                if (disposing)
                {
                    GL.DeleteProgram(Handle);
                }
                else Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?"); // If the deconstructor called Dispose()

                disposedValue = true;
            }
        }

        ~Shader()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
