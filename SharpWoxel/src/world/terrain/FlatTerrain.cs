using OpenTK.Mathematics;
using SharpWoxel.World.Blocks;

namespace SharpWoxel.World.Terrain;

class FlatTerrain : BaseTerrain
{
    public FlatTerrain(Vector3i size, Vector3i chunkSize)
        : base(size, chunkSize)
    {
    }

    protected override void Generate()
    {
        int maxAmp = _chunkSize.Y / 2;

        foreach (var chunk in _chunks)
        {
            for (int z = 0; z < _chunkSize.Z; z++)
            {
                for (int y = 0; y < _chunkSize.Y; y++)
                {
                    for (int x = 0; x < _chunkSize.X; x++)
                    {
                        if (y <= 1)
                        {
                            chunk.SetBlockLocal(x, y, z, new GrassBlock());
                        }
                    }
                }
            }
            chunk.SetBlockLocal(1, 2, 1, new DirtBlock());
            chunk.SetBlockLocal(1, 1, 1, new AirBlock());
            chunk.SetBlockLocal(1, 1, 0, new AirBlock());
            chunk.SetBlockLocal(0, 1, 0, new AirBlock());
            chunk.SetBlockLocal(0, 1, 2, new StoneBlock());
            chunk.SetBlockLocal(0, 1, 1, new DirtBlock());
            chunk.SetBlockLocal(0, 1, 3, new LeafBlock());
            chunk.SetBlockLocal(0, 1, 4, new PlankBlock());

            // chunk.BuildMesh(); Don't do this, only build meshes after all the chunks blocks have been set in every chunk
            // chunk.BuildMesh() is called in GenerateTerrain() after the Generate() function call which should set chunk blocks
        }
    }
}
