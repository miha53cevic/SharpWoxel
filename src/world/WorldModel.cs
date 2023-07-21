using glObjects;
using SharpWoxel.util;
using SharpWoxel.world.terrain;

namespace SharpWoxel.world
{
    class WorldModel
    {
        private Terrain _terrain;
        private List<IWorldModelListener> _listeners;

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

        public void notify()
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
    }
}
