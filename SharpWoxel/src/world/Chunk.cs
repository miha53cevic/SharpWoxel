using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.util;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world;

internal enum NeighbourDirection
{
    North,
    South,
    East,
    West,
    Above,
    Below
}

internal class Chunk
{
    private const int NeighbourCount = 6;
    private readonly IBlock[,,] _blocks;
    private readonly Chunk[] _neighbours;
    private readonly Vector3i _chunkSize;

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

        /*int i = 0;
        foreach (var n in _neighbours)
        {
            Console.WriteLine(string.Format("{0}: {1}", (NeighbourDirection)i, n));
            i++;
        }
        Console.WriteLine(Entity.Position);
        Console.WriteLine(this);*/

        for (var x = 0; x < _chunkSize.X; x++)
        for (var y = 0; y < _chunkSize.Y; y++)
        for (var z = 0; z < _chunkSize.Z; z++)
        {
            // Skip if air block
            if (_blocks[x, y, z].IsAir())
                continue;

            // Check for possible block neighbours
            bool left = false, right = false, front = false, back = false, top = false, bottom = false;
            if (x != 0) left = true;
            if (y != 0) bottom = true;
            if (z != 0) back = true;
            if (x != _chunkSize.X - 1) right = true;
            if (y != _chunkSize.Y - 1) top = true;
            if (z != _chunkSize.Z - 1) front = true;

            // Create cube face if the neighbouring block is air
            if (left)
                if (_blocks[x - 1, y, z].IsAir())
                    CreateCubeFace(Cube.Face.Left, x, y, z);
            if (right)
                if (_blocks[x + 1, y, z].IsAir())
                    CreateCubeFace(Cube.Face.Right, x, y, z);
            if (bottom)
                if (_blocks[x, y - 1, z].IsAir())
                    CreateCubeFace(Cube.Face.Bottom, x, y, z);
            if (top)
                if (_blocks[x, y + 1, z].IsAir())
                    CreateCubeFace(Cube.Face.Top, x, y, z);
            if (back)
                if (_blocks[x, y, z - 1].IsAir())
                    CreateCubeFace(Cube.Face.Back, x, y, z);
            if (front)
                if (_blocks[x, y, z + 1].IsAir())
                    CreateCubeFace(Cube.Face.Front, x, y, z);

            // Check 1 block width with neighbouring chunks, for the outer chunk blocks
            if (!left && _neighbours.GetValue((int)NeighbourDirection.West) != null)
                if (_neighbours[(int)NeighbourDirection.West].GetBlockLocal(_chunkSize.X - 1, y, z).IsAir())
                    CreateCubeFace(Cube.Face.Left, x, y, z);
            if (!right && _neighbours.GetValue((int)NeighbourDirection.East) != null)
                if (_neighbours[(int)NeighbourDirection.East].GetBlockLocal(0, y, z).IsAir())
                    CreateCubeFace(Cube.Face.Right, x, y, z);
            if (!bottom && _neighbours.GetValue((int)NeighbourDirection.Below) != null)
                if (_neighbours[(int)NeighbourDirection.Below].GetBlockLocal(x, _chunkSize.Y - 1, z).IsAir())
                    CreateCubeFace(Cube.Face.Bottom, x, y, z);
            if (!top && _neighbours.GetValue((int)NeighbourDirection.Above) != null)
                if (_neighbours[(int)NeighbourDirection.Above].GetBlockLocal(x, 0, z).IsAir())
                    CreateCubeFace(Cube.Face.Top, x, y, z);
            if (!back && _neighbours.GetValue((int)NeighbourDirection.North) != null)
                if (_neighbours[(int)NeighbourDirection.North].GetBlockLocal(x, y, _chunkSize.Z - 1).IsAir())
                    CreateCubeFace(Cube.Face.Back, x, y, z);
            if (!front && _neighbours.GetValue((int)NeighbourDirection.South) != null)
                if (_neighbours[(int)NeighbourDirection.South].GetBlockLocal(x, y, 0).IsAir())
                    CreateCubeFace(Cube.Face.Front, x, y, z);
        }

        Entity.SetVerticies(tempVerticies.ToArray(), BufferUsageHint.DynamicDraw);
        Entity.SetIndicies(tempIndicies.ToArray(), BufferUsageHint.DynamicDraw);
        Entity.SetTextureCoords(tempTextureCoords.ToArray(), BufferUsageHint.DynamicDraw);

        Console.WriteLine("Created {0} with: verticies({1}), indicies({2}), textureCoords({3})", this,
            tempVerticies.Count, tempIndicies.Count, tempTextureCoords.Count);
        return;

        void CreateCubeFace(Cube.Face face, int x, int y, int z)
        {
            var faceVerticies = Cube.CubeFace.GetCubeFace(face);

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