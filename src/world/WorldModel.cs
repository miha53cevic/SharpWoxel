using glObjects;
using OpenTK.Mathematics;
using SharpWoxel.util;
using SharpWoxel.world.blocks;
using SharpWoxel.world.terrain;

namespace SharpWoxel.world
{
    class WorldModel
    {
        private Terrain _terrain;
        private List<IWorldModelListener> _listeners;

        public Terrain Terrain { get => _terrain; set => _terrain = value; }

        public WorldModel(Terrain terrain)
        {
            _terrain = terrain;
            _listeners = new List<IWorldModelListener>();
        }

        // Observer code
        public void Subscribe(IWorldModelListener listener)
        {
            _listeners.Add(listener);
        }

        public void Unsubscribe(IWorldModelListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Notify()
        {
            foreach (var listener in _listeners)
            {
                listener.Update(this);
            }
        }

        public void Render(Shader shader, Camera camera)
        {
            _terrain.Render(shader, camera);
        }

        public void GenerateWorld()
        {
            _terrain.GenerateTerrain();
        }

        public void LoadWorld(string savePath)
        {

        }

        public void Update(double deltaTime)
        {

        }

        public Vector3i? CastRayFirstBlockIntersection(Camera camera, float rayMaxLength = 6f, float rayStep = 0.01f)
        {
            for (Ray ray = new Ray(camera.Position, camera.Front); ray.GetLength() < rayMaxLength; ray.Step(rayStep))
            {
                var end = ray.GetEnd();
                int x = (int)end.X;
                int y = (int)end.Y;
                int z = (int)end.Z;
                // Check if ray is going outside of the world
                if (x < 0 || x >= _terrain.TerrainSize.X * _terrain.ChunkSize.X ||
                    y < 0 || y >= _terrain.TerrainSize.Y * _terrain.ChunkSize.Y ||
                    z < 0 || z >= _terrain.TerrainSize.Z * _terrain.ChunkSize.Z)
                    break;
                // Get the block, and if it isn't air stop the raycast
                var block = _terrain.GetBlockGlobal(x, y, z);
                if (!block.IsAir())
                {
                    return (x, y, z);
                }
            }
            return null;
        }
    }
}
