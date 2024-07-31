using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.mesh;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world;

internal enum NeighbourDirection
{
    West = 0,
    East = 1,
    South = 2,
    North = 3,
    Above = 4,
    Below = 5,
}

internal class Chunk
{
    private const int NeighbourCount = 6;
    private readonly IBlock[,,] _blocks;
    private readonly Vector3i _chunkSize;
    private readonly Chunk[] _neighbours;

    public Chunk(Vector3 position, Vector3i chunkSize)
    {
        Entity = new ChunkEntity
        {
            Position = position
        };

        _chunkSize = chunkSize;
        _blocks = new IBlock[chunkSize.X, chunkSize.Y, chunkSize.Z]; // default object values is null
        _neighbours = new Chunk[NeighbourCount];

        // Set chunk to be air by default
        AirOutChunk();
    }

    public ChunkEntity Entity { get; set; }

    public void AirOutChunk()
    {
        for (var x = 0; x < _chunkSize.X; x++)
        for (var y = 0; y < _chunkSize.Y; y++)
        for (var z = 0; z < _chunkSize.Z; z++)
            _blocks[x, y, z] = new AirBlock();
    }

    public IBlock GetBlockLocal(int x, int y, int z)
    {
        if (IsBlockOutOfBounds(x, y, z))
            throw new Exception($"[Chunk::GetBlockLocal]: Block ({x},{y},{z}) is out of bounds");

        return _blocks[x, y, z];
    }

    public void SetBlockLocal(int x, int y, int z, IBlock block)
    {
        if (IsBlockOutOfBounds(x, y, z))
            throw new Exception($"[Chunk::SetBlockLocal]: Block ({x},{y},{z}) is out of bounds");

        _blocks[x, y, z] = block;
    }

    private bool IsBlockOutOfBounds(int x, int y, int z)
    {
        return x < 0 || x >= _chunkSize.X || y < 0 || y >= _chunkSize.Y || z < 0 || z >= _chunkSize.Z;
    }

    public void BuildMesh()
    {
        var tempVerticies = new List<float>();
        var tempTextureCoords = new List<float>();
        var tempIndicies = new List<uint>();
        uint indicies = 0;

        int[][] blockNeighbourLocations =
        [
            [-1, 0, 0], // left
            [1, 0, 0], // right
            [0, 0, 1], // front
            [0, 0, -1], // back
            [0, 1, 0], // top
            [0, -1, 0], // bottom
        ];

        // -1 za koordinate koje se ne diraju (x, y, 0) => (-1, -1, 0)
        int[][] neighboursBlockLocations =
        [
            [_chunkSize.X - 1, -1, -1], // left
            [0, -1, -1], // right
            [-1, -1, 0], // front
            [-1, -1, _chunkSize.Z - 1], // back
            [-1, 0, -1], // top
            [-1, _chunkSize.Y - 1, -1], // bottom
        ];

        for (var x = 0; x < _chunkSize.X; x++)
        for (var y = 0; y < _chunkSize.Y; y++)
        for (var z = 0; z < _chunkSize.Z; z++)
        {
            // Skip if air block
            if (_blocks[x, y, z].IsAir())
                continue;

            // Check for possible block neighbours
            var blockNeighbourExists = new bool[6];
            // bool left = false, right = false, front = false, back = false, top = false, bottom = false;
            if (x != 0) blockNeighbourExists[0] = true;
            if (y != 0) blockNeighbourExists[5] = true;
            if (z != 0) blockNeighbourExists[3] = true;
            if (x != _chunkSize.X - 1) blockNeighbourExists[1] = true;
            if (y != _chunkSize.Y - 1) blockNeighbourExists[4] = true;
            if (z != _chunkSize.Z - 1) blockNeighbourExists[2] = true;

            // Create cube face if the neighbouring block is air
            for (var i = 0; i < blockNeighbourExists.Length; i++)
            {
                if (!blockNeighbourExists[i]) continue; // nema susjedne kocke s te strane

                var blockNeighbourLocation = blockNeighbourLocations[i];
                var blockNeighbourX = blockNeighbourLocation[0] + x;
                var blockNeighbourY = blockNeighbourLocation[1] + y;
                var blockNeighbourZ = blockNeighbourLocation[2] + z;
                if (_blocks[blockNeighbourX, blockNeighbourY, blockNeighbourZ]
                    .IsAir())
                    CreateCubeFace((CubeFaceMesh.Face)i, x, y, z);
            }

            // Check 1 block width with neighbouring chunks, for the outer chunk blocks
            for (var i = 0; i < blockNeighbourExists.Length; i++)
            {
                if (blockNeighbourExists[i])
                    continue; // ako nema susjeda je rubna kocka u svojem chunk-u, inace preskoci
                if (_neighbours.GetValue(i) == null) continue; // ako smo rubna kocka i nema chunk susjeda onda preskoci

                var neighboursBlockLocation = neighboursBlockLocations[i];
                var neighbourBlockX = neighboursBlockLocation[0] == -1 ? x : neighboursBlockLocation[0];
                var neighbourBlockY = neighboursBlockLocation[1] == -1 ? y : neighboursBlockLocation[1];
                var neighbourBlockZ = neighboursBlockLocation[2] == -1 ? z : neighboursBlockLocation[2];
                if (_neighbours[i]
                    .GetBlockLocal(neighbourBlockX, neighbourBlockY, neighbourBlockZ)
                    .IsAir())
                    CreateCubeFace((CubeFaceMesh.Face)i, x, y, z);
            }
        }

        Entity.Mesh.RebuildMesh(
            tempVerticies.ToArray(),
            tempIndicies.ToArray(),
            tempTextureCoords.ToArray(),
            BufferUsageHint.DynamicDraw
        );

        Console.WriteLine("Created {0} with: verticies({1}), indicies({2}), textureCoords({3})", this,
            tempVerticies.Count, tempIndicies.Count, tempTextureCoords.Count);
        return;

        void CreateCubeFace(CubeFaceMesh.Face face, int x, int y, int z)
        {
            var faceVerticies = CubeFaceMesh.GetCubeFaceVerticies(face);

            for (var i = 0; i < faceVerticies.Length / 3; i++)
            {
                tempVerticies.Add(faceVerticies[0 + i * 3] + x);
                tempVerticies.Add(faceVerticies[1 + i * 3] + y);
                tempVerticies.Add(faceVerticies[2 + i * 3] + z);
            }

            var block = _blocks[x, y, z];
            var coords = block.GetFaceTextureAtlasCoordinates(face);
            tempTextureCoords.AddRange(ChunkEntity.TexAtlas.GetTextureCoords(coords.X, coords.Y));

            tempIndicies.Add(indicies + 0);
            tempIndicies.Add(indicies + 1);
            tempIndicies.Add(indicies + 3);
            tempIndicies.Add(indicies + 3);
            tempIndicies.Add(indicies + 1);
            tempIndicies.Add(indicies + 2);
            indicies += 4;
        }
    }

    public void RebuildMesh()
    {
        BuildMesh();
        RebuildNeighbours();
    }

    public void SetNeighbour(NeighbourDirection direction, Chunk neighbour)
    {
        _neighbours[(int)direction] = neighbour;
    }

    private void RebuildNeighbours()
    {
        foreach (var neighbour in _neighbours)
            neighbour.BuildMesh(); // vazno da NIJE rebuild onda je rekurzivno zvanje
    }

    public override string ToString()
    {
        return $"Chunk({GetHashCode()}, {Entity.Position})";
    }
}