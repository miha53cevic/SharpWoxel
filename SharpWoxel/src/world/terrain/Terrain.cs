using OpenTK.Mathematics;
using SharpWoxel.GLObjects;
using SharpWoxel.util;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world.terrain;

internal abstract class BaseTerrain
{
    public readonly List<Chunk> ChunksList;
    public Vector3i NumberOfChunks;
    public Vector3i ChunkSize;

    protected BaseTerrain(Vector3i numberOfChunks, Vector3i chunkSize)
    {
        ChunksList = new List<Chunk>();
        NumberOfChunks = numberOfChunks; // number of chunks
        ChunkSize = chunkSize;

        // Create chunks
        // Bitan redosljed za IndexFrom3D, mora biti x,y,z for loop order
        for (var x = 0; x < numberOfChunks.X; x++)
        for (var y = 0; y < numberOfChunks.Y; y++)
        for (var z = 0; z < numberOfChunks.Z; z++)
        {
            Vector3 position = new Vector3i(chunkSize.X * x, chunkSize.Y * y, chunkSize.Z * z);
            ChunksList.Add(new Chunk(position, chunkSize));
        }

        // Set each chunks neighbours
        for (var x = 0; x < numberOfChunks.X; x++)
        for (var y = 0; y < numberOfChunks.Y; y++)
        for (var z = 0; z < numberOfChunks.Z; z++)
        {
            bool left = false,
                right = false,
                front = false,
                back = false,
                top = false,
                bottom = false; // hasNeighbouring chunk
            if (x != 0) left = true;
            if (y != 0) bottom = true;
            if (z != 0) back = true;
            if (x != numberOfChunks.X - 1) right = true;
            if (y != numberOfChunks.Y - 1) top = true;
            if (z != numberOfChunks.Z - 1) front = true;

            // boolean values tell us if a given chunk can have a neighbour on that side and if it can
            // we add a reference to that neighbour to the given chunk
            if (left)
                GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.West, GetChunkFromLocal(x - 1, y, z));
            if (right)
                GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.East, GetChunkFromLocal(x + 1, y, z));
            if (back)
                GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.North, GetChunkFromLocal(x, y, z - 1));
            if (front)
                GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.South, GetChunkFromLocal(x, y, z + 1));
            if (top)
                GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.Above, GetChunkFromLocal(x, y + 1, z));
            if (bottom)
                GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.Below, GetChunkFromLocal(x, y - 1, z));
        }
    }

    // Fill out chunk blocks, chunk.BuildMesh() is called after this function in GenerateTerrain()
    protected abstract void Generate();

    // okvirna metoda
    public virtual void GenerateTerrain()
    {
        Generate();
        foreach (var chunk in ChunksList) chunk.BuildMesh();
    }

    public void Render(Shader shader, Camera camera)
    {
        foreach (var chunk in ChunksList) chunk.Entity.Render(shader, camera);
    }

    public IBlock GetBlockGlobal(int x, int y, int z)
    {
        // Convert to local block position
        var lx = x % ChunkSize.X;
        var ly = y % ChunkSize.Y;
        var lz = z % ChunkSize.Z;

        return GetChunkFromGlobal(x, y, z).GetBlockLocal(lx, ly, lz);
    }

    public void SetBlockGlobal(int x, int y, int z, IBlock block)
    {
        // Convert to local block position
        var lx = x % ChunkSize.X;
        var ly = y % ChunkSize.Y;
        var lz = z % ChunkSize.Z;

        GetChunkFromGlobal(x, y, z).SetBlockLocal(lx, ly, lz, block);
    }

    public Chunk GetChunkFromGlobal(int x, int y, int z)
    {
        // Get the chunk from global block position
        var cx = x / ChunkSize.X;
        var cy = y / ChunkSize.Y;
        var cz = z / ChunkSize.Z;

        if (IsChunkOutOfBounds(cx, cy, cz))
            throw new Exception(string.Format("[Terrain::GetChunkFromGlobal]: Chunk ({0},{1},{2}) is out of bounds", cx,
                cy, cz));

        return ChunksList[Maths.IndexFrom3D(cx, cy, cz, NumberOfChunks.Y, NumberOfChunks.Z)];
    }

    public Chunk GetChunkFromLocal(int x, int y, int z)
    {
        if (IsChunkOutOfBounds(x, y, z))
            throw new Exception($"[Terrain::GetChunkFromLocal]: Chunk ({x},{y},{z}) is out of bounds");

        return ChunksList[Maths.IndexFrom3D(x, y, z, NumberOfChunks.Y, NumberOfChunks.Z)];
    }

    private bool IsChunkOutOfBounds(int x, int y, int z)
    {
        return x < 0 || x >= NumberOfChunks.X || y < 0 || y >= NumberOfChunks.Y || z < 0 || z >= NumberOfChunks.Z;
    }
}