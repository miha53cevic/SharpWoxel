using OpenTK.Mathematics;
using SharpWoxel.GLObjects;
using SharpWoxel.util;
using SharpWoxel.world.blocks;
using SharpWoxel.world.terrain;

namespace SharpWoxel.world;

internal class WorldModel(BaseTerrain terrain)
{
    private readonly List<IWorldModelListener> _listeners = [];

    public BaseTerrain Terrain { get; } = terrain;

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
        foreach (var listener in _listeners) listener.Update(this);
    }

    public void Render(Shader shader, Camera camera)
    {
        Terrain.Render(shader, camera);
    }

    public void GenerateWorld()
    {
        Terrain.GenerateTerrain();
    }

    public void LoadWorld(string savePath)
    {
    }

    public void Update(double deltaTime)
    {
    }

    public Vector3i? CastRayToFirstBlock(Camera camera, float rayMaxLength = 6f, float rayStep = 0.01f)
    {
        for (var ray = new Ray(camera.Position, camera.Front); ray.GetLength() < rayMaxLength; ray.Step(rayStep))
        {
            var end = ray.GetEnd();
            var x = (int)end.X;
            var y = (int)end.Y;
            var z = (int)end.Z;
            // Check if ray is going outside of the world
            if (x < 0 || x >= Terrain.NumberOfChunks.X * Terrain.ChunkSize.X ||
                y < 0 || y >= Terrain.NumberOfChunks.Y * Terrain.ChunkSize.Y ||
                z < 0 || z >= Terrain.NumberOfChunks.Z * Terrain.ChunkSize.Z)
                break;
            // Get the block, and if it isn't air stop the raycast
            var block = Terrain.GetBlockGlobal(x, y, z);
            if (!block.IsAir()) return (x, y, z);
        }

        return null;
    }

    public void PlaceBlock(Vector3i position, IBlock block)
    {
        var chunk = Terrain.GetChunkFromGlobal(position.X, position.Y, position.Z);
        Terrain.SetBlockGlobal(position.X, position.Y, position.Z, block);
        chunk.RebuildMesh();
    }

    public void BreakBlock(Vector3i position)
    {
        PlaceBlock(position, new AirBlock());
    }
}