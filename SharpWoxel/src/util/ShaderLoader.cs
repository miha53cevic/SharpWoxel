using SharpWoxel.GLObjects;

namespace SharpWoxel.util;

internal class ShaderLoader
{
    private static readonly ShaderLoader Instance = new();
    private readonly Dictionary<string, Shader> _loadedShaders;

    static ShaderLoader()
    {
    }

    private ShaderLoader()
    {
        _loadedShaders = new Dictionary<string, Shader>();
    }

    public static ShaderLoader GetInstance()
    {
        return Instance;
    }

    // Shader path withouth the .vert/.frag extension
    public void Load(string path)
    {
        var name = path.Split("/").Last();

        var shader = new Shader(path + ".vert", path + ".frag");
        _loadedShaders.Add(name, shader);

        Console.WriteLine("[ShaderLoader]: Loaded {0} shader", name);
    }

    public Shader GetShader(string name)
    {
        var exists = _loadedShaders.ContainsKey(name);
        if (!exists)
            throw new Exception($"Shader: {name} does not exist in loadedShaders");
        return Instance._loadedShaders[name];
    }

    public void Destroy()
    {
        foreach (var entry in _loadedShaders) entry.Value.Dispose();
        Instance._loadedShaders.Clear();
    }
}