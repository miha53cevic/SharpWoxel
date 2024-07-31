using OpenTK.Mathematics;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world.terrain;

internal class FlatTerrain(Vector3i numberOfChunks, Vector3i chunkSize) : BaseTerrain(numberOfChunks, chunkSize)
{
    protected override void Generate()
    {
        var maxAmp = chunkSize.Y / 2;

        foreach (var chunk in ChunksList)
        {
            for (var z = 0; z < chunkSize.Z; z++)
            for (var y = 0; y < chunkSize.Y; y++)
            for (var x = 0; x < chunkSize.X; x++)
                if (y <= 1)
                    chunk.SetBlockLocal(x, y, z, new GrassBlock());
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