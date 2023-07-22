
using glObjects;

namespace SharpWoxel.util
{
    class ShaderLoader
    {
        private static readonly ShaderLoader _instance = new ShaderLoader();
        private readonly Dictionary<string, Shader> _loadedShaders;

        static ShaderLoader()
        {
        }

        private ShaderLoader()
        {
            _loadedShaders = new Dictionary<string, Shader>();
        }

        public static ShaderLoader GetInstance() { return _instance; }

        // Shader path withouth the .vert/.frag extension
        public void Load(string path)
        {
            string name = path.Split("/").Last();
            
            Shader shader = new Shader(path + ".vert", path + ".frag");
            _loadedShaders.Add(name, shader);

            Console.WriteLine(string.Format("[ShaderLoader]: Loaded {0} shader", name));
        }

        public Shader GetShader(string name) 
        {
            bool exists = _loadedShaders.ContainsKey(name);
            if (!exists)
                throw new Exception(string.Format("Shader: {0} does not exist in loadedShaders", name));
            return _instance._loadedShaders[name];
        }

        public void Destroy()
        {
            foreach (var entry in _loadedShaders)
            {
                entry.Value.Dispose();
            }
            _instance._loadedShaders.Clear();
        }
    }
}
