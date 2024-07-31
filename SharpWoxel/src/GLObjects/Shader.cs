using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SharpWoxel.GLObjects;

internal sealed class Shader : IDisposable
{
    private readonly string _fragmentPath;
    private readonly string _vertexPath;
    private bool _disposedValue;
    private int _handle = -1;

    public Shader(string vertexPath, string fragmentPath)
    {
        _vertexPath = vertexPath;
        _fragmentPath = fragmentPath;

        CompileShader();
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void CompileShader()
    {
        var vertexShaderSrc = File.ReadAllText(_vertexPath);
        var fragmentShaderSrc = File.ReadAllText(_fragmentPath);

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSrc);

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSrc);

        int success;
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            var infoLog = GL.GetShaderInfoLog(vertexShader);
            throw new Exception(infoLog);
        }

        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            var infoLog = GL.GetShaderInfoLog(fragmentShader);
            throw new Exception(infoLog);
        }

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0) throw new Exception("[Shader]: Link error");

        // Cleanup, since they are linked now to the shader program
        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var count);
        Console.WriteLine("[Shader]: Loaded {0} uniforms", count);
    }

    public void Reload()
    {
        Console.WriteLine("[Shader]: Hot Reload {0}, {1}", _vertexPath, _fragmentPath);

        var oldProgramHandle = _handle;
        try
        {
            CompileShader();
            GL.DeleteProgram(oldProgramHandle);
        }
        catch (Exception ex)
        {
            _handle = oldProgramHandle;
            Console.WriteLine("[Shader]: Could not compile new shader source on hot reload, {0}", ex.Message);
        }
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public int GetUniformLocation(string name)
    {
        if (_handle == -1) throw new Exception("[Shader]: _handle is -1");

        var location = GL.GetUniformLocation(_handle, name);
        if (location == -1)
            throw new Exception($"[Shader]: Found no location for the uniform: {name}");

        return location;
    }

    public void SetMatrix4(int location, Matrix4 mat)
    {
        Use();
        GL.UniformMatrix4(location, true, ref mat);
    }

    public void SetInt(int location, int data)
    {
        Use();
        GL.Uniform1(location, data);
    }

    public void SetFloat(int location, float data)
    {
        Use();
        GL.Uniform1(location, data);
    }

    public void SetVector3(int location, Vector3 data)
    {
        Use();
        GL.Uniform3(location, data);
    }

    public void SetVector2(int location, Vector2 data)
    {
        Use();
        GL.Uniform2(location, data);
    }

    private void Dispose(bool disposing)
    {
        // If disposed is called twice, only run if value has not been already disposed
        if (!_disposedValue)
        {
            // If the user called Dispose() on the object
            if (disposing)
                GL.DeleteProgram(_handle);
            else
                throw new Exception(
                    "GPU Resource leak! Did you forget to call Dispose()?"); // If the deconstructor called Dispose()

            _disposedValue = true;
        }
    }

    ~Shader()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(false);
    }
}